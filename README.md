Raporlama Servisi
Harici bir api servisinden raporlama yapmak için geliştirilmiş uygulama.

Gereklilikler
PostgreSQL
Apache ZooKeeper
Apache Kafka
.Net 6.0
UnitOfWork Design Pattern
Yapılandırmalar: Web servislerinin ayarları ContactMicroService.WebApi ve ReportMicroService.WebApi projeleri altındaki appsettings.json dosyasında saklanır.

Veritabanı yapılandırmasını "appsettings.json" altındaki "ConnectionStrings:Default" altında düzenleyebilirsiniz. Ek olarak DbMigrator uygulaması veritabanı migration işlemini kolaylaştırmaya yarayan bir CommandLineTool'dur. Bu uygulamayı çalıştırarak migration işlemini hızlıca halledebilirsin.

ContactMicroService tarafında yapıldığı şekilde ReportMicroService'de de veritabanı yapılandırma ayarları ayarlandıktan sonra, bu serviste Kafka kullanacağımız için onun yapılandırma ayarlarını da *.WebApi altındaki "appsettings.json" dosyasına ekliyoruz. Gerekli yapılandırmalar şu şekilde:

![image](https://user-images.githubusercontent.com/86706539/190571462-85c8064c-b2ba-4e77-b997-d5fefcf84d3c.png)

BootstrapServers: Kafka sunucusunun erişim adresini tanımlar.
GroupId: Üzerinde çalışılacak kafka grubunu belirler.
TopicName: Kafka veri iletişiminin sağlanacağı konuyu belirler.
ContactMicroService
ContactMicroService .Net Core çatısı altında geliştirilip, veritabanı yönetim sistemi olarak PostgreSQL, ORM olarak ise EntityFrameworkCore kullanıldı. Api şablonuna erişmek için dahili swagger aktif, kullanılabilir durumda. Servisin ana görevi bir rehber uygulamasına api hizmeti vermektir. Built-in özellikleri haricinde "rehber" özelliğini yerine getirmesi için oluşturulan "Contact" ve "ContactInfo" nesnelerini barındırır.

Contact nesnesi aşağıdaki alanları barındırır:
FirstName: Kişinin ismini barındırır.
LastName: Kişinin soyismini barındırır.
Company: Kişinin şirket bilgisini barındırır.
ContactInfos: Kişiye atanmış iletişim bilgilerini listeli olarak saklar. (Contact 1:N ContactInfo ilişkisine sahiptir.)
Contact hizmeti temel CRUD işlemleri hariç şu özellikleri barındırır:
![image](https://user-images.githubusercontent.com/86706539/190571994-5296126e-4baa-4bf3-bf87-1d967c7c8ae4.png)

ContactDetails: Id si gönderilen kaydı iletişim bilgileriyle birlikte döner.

FilteredList: Sayfalamayı destekleyerek veritabanındaki tüm verileri döner. Ek olarak iletişim bilgisine göre filtreleme yapabilir. Parametreler:

ContactType: Filtrelenmek istenen iletişim bilgisini belirleyen parametredir. (Undefined=0 , PhoneNumber=1 , Mail=2 , Location=3)
Information: ContactType'da belirlenen tipin verisinin saklandığı alandır. Filtrelemek için sadece tip değil bilgi de seçilebilir.
Sorting: "Pagination" özelliğinin sıralanma özelliğinin parametresidir.
SkipCount: "Pagination" özelliğinin dönen verilerin başlangıç noktasını belirleyen parametredir.
MaxResultCount: "Pagination" özelliği ile tek seferde en fazla kaç veri geleceğini belirleyen parametredir.

ReportData: Konuma göre filtrelenmiş bir şekilde konumu,o konumdaki insan sayısını ve yine o konumdaki kayıtlı telefon numarası sayısını döner. Parametreler:
Location : Hedef konumu belirlemek için kullanılan parametre.

ContactInfo nesnesi içerisinde aşağıdaki alanları barındırır:

ContactId: Contact tablosu ile 1:N ilişkiyi oluşturabilmek için bağlı olduğu "Contact" elemanının Id'sinin saklandığı alandır.
Information: İletişim bilgisinin saklandığı alandır.
ContactType: Saklanan iletişim bilgisinin tipini belirleyen alandır. (Undefined=0 , PhoneNumber=1 , Mail=2 , Location=3)
ReportMicroService
ReportService .NET çatısı altında geliştirilip,Veritabanı yönetim sistemi olarak PostgreSQL, ORM olarak ise EntityFrameworkCore kullanıldı. Api şablonuna erişmek için dahili swagger aktif, kullanılabilir durumda. Servisin ana görevi gelen istek doğrultusunda uzak api sunucusuna erişip Excel dosyası olarak raporlamaktır. Built-in özellikleri haricinde ExportToExcelService ve ContactReportBackgroundWorker servisleri vardır.

ExportToExcel
Girdi olarak verilen nesneyi Excel olarak çıktı verir.

ContactReportBackgroundWorker
Rapor isteği yapıldıktan sonra raporun hazırlanıp excel haline getirmekten sorumlu servis.

Report nesnesi içerisinde aşağıdaki alanları barındırır:
ReportState: Raporun durumu hakkında bilgi saklar. (Preparing=0,Completed=1)
ReportURL: Rapor sonucu elde edilen excelin indirilebileceği linki saklar.
Report hizmeti temel CRUD işlemleri hariç şu özellikleri barındırır:
Location: Raporlamanın yapılacağı konum bilgisini alır.
