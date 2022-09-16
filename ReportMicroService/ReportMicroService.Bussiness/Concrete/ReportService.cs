using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReportMicroService.Bussiness.Abstract;
using ReportMicroService.Entities.Enums;
using ReportMicroService.Entities.Model;
using ReportMicroService.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroService.Bussiness.Concrete
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IReportService> _logger;
        public ReportService(IUnitOfWork unitOfWork,ILogger<IReportService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Report> CreateReport(Report report)
        {
            await _unitOfWork.Report.AddAsync(report);
            await _unitOfWork.CommitAsync();
            return report;
        }

        public async Task DeleteReport(Report report)
        {
            _unitOfWork.Report.Remove(report);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Report>> GetAllReports()
        {
            return await _unitOfWork.Report.GetAllAsync();
        }

        public async Task<Report> GetReportById(int id)
        {
            return await _unitOfWork.Report.GetByIdAsync(id);
        }

        public async Task<Report> RequestReport(string Location)
        {
            var reportItem = new Report
            {
                ReportState = ReportState.Preparing
            };
            var report = await CreateReport(reportItem);

            ReportRequest request = new ReportRequest { 
                Id=report.ReportId,
                Location=Location
            };
            await SendDataKafkaProducer(JsonConvert.SerializeObject(request));
            return report;
        }

        public async Task UpdateReport(Report report)
        {
            _unitOfWork.Report.Update(report);
            await _unitOfWork.CommitAsync();
        }

        private async Task SendDataKafkaProducer(string apiResponse)
        {
            var builderConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfigurationRoot _config = builderConfig.Build();
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:Connections:Default:BootstrapServers"],
                ClientId = Dns.GetHostName(),
            };

            var topic = _config["Kafka:EventBus:TopicName"];
            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var data = await producer.ProduceAsync(topic, new Message<Null, string>
                    {
                        Value = apiResponse
                    }); ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Send Kafka Data error: " + ex.Message);
            }
        }
    }
}
