using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Confluent.Kafka;
using ReportMicroService.Bussiness.Abstract;
using Microsoft.Extensions.Hosting;
using ReportMicroService.Entities.Model;
using Microsoft.AspNetCore.WebUtilities;
using ReportMicroService.Entities.Enums;
using ReportMicroService.Reports;
using Microsoft.Extensions.Configuration.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using ReportMicroService.Bussiness.Concrete;

namespace ReportMicroService.ContactReports
{
    public class ContactReportBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IHostEnvironment Environment;
        public ContactReportBackgroundWorker(IServiceProvider serviceProvider, IHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider;
            Environment = webHostEnvironment;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => ExecuteAsync(cancellationToken));
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var builderConf = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConfigurationRoot _configuration = builderConf.Build();
            var config = new ConsumerConfig
            {
                GroupId = _configuration["Kafka:EventBus:GroupId"],
                BootstrapServers = _configuration["Kafka:Connections:Default:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(_configuration["Kafka:EventBus:TopicName"]);
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumer = consumerBuilder.Consume();
                        var data = consumer.Message.Value;
                        await DoWork(data);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Kafka background consumer error: " + ex.Message);
            }
        }

        private async Task DoWork(string requestData)
        {
            try
            {

                var reportRequestData = JsonConvert.DeserializeObject<ReportRequest>(requestData);


                Dictionary<string, string> query = new Dictionary<string, string>();
                query.Add("Location", reportRequestData.Location);
                ContactReport report = new ContactReport();


                string uri = QueryHelpers.AddQueryString("https://localhost:7116/api/Contact/GetContactReportByLocation", query);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(uri))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        report = JsonConvert.DeserializeObject<ContactReport>(apiResponse);
                    }
                }

                if (report != null)
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        IReportService scopedProcessingService =
                            scope.ServiceProvider.GetRequiredService<IReportService>();

                        var item = await scopedProcessingService.GetReportById(reportRequestData.Id);
                        if (item != null)
                        {
                            var rootPath = Path.Combine(this.Environment.ContentRootPath, "wwwroot");
                            var filePath = Path.Combine("contents", @"ContactReport_" + DateTime.Now.ToString("MM.dd.yyyyTHH.mm") + ".xlsx".Trim());
                            var absolutePath = Path.Combine(rootPath, filePath);
                            var exportService = new ExportToExcelService();
                            var excelSource = await exportService.ExportToExcel(report);
                            exportService.SaveAsFile(excelSource, absolutePath);
                            var updatedItem = new Report();

                            item.ReportURL = filePath;
                            item.ReportState = ReportState.Completed;

                            await scopedProcessingService.UpdateReport(item);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Backworker runtime error: " + ex.Message);
            }
        }
    }
}
