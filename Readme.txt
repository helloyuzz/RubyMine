从数据库生成Model文件：
Scaffold-DbContext "Server=localhost;Port=3306;Database=bitnami_redmineplusagile;Uid=bitnami;Pwd=e32a9b7bd9;SslMode=None;AllowPublicKeyRetrieval=true;" Pomelo.EntityFrameworkCore.Mysql -f -o Models

Scaffold-DbContext "Server=localhost;Port=3306;Database=bitnami_redmineplusagile;Uid=bitnami;Pwd=e32a9b7bd9;SslMode=None;AllowPublicKeyRetrieval=true;" Pomelo.EntityFrameworkCore.Mysql -Tables tracker_mapping -f -o Models