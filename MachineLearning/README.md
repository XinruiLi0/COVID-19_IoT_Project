AWS Sage Maker Jupyter book ODBC installation Instruction:

Open terminal in Sage Maker notebook

command:

$ sudo su
$ curl https://packages.microsoft.com/config/rhel/6/prod.repo > /etc/yum.repos.d/mssql-release.repo
$ exit
$ sudo yum update
$ sudo yum remove unixODBC #to avoid conflicts
$ sudo ACCEPT_EULA=Y yum install msodbcsql-13.0.1.0-1 mssql-tools-14.0.2.0-1
$ sudo yum install unixODBC-utf16-devel

reference:https://aws.amazon.com/premiumsupport/knowledge-center/odbc-driver-sagemaker-sql-server/

