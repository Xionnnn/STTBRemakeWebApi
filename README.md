# **STTB Remake Web API**
## Overview
This is the Api for STTB Profile Website (STTB Institutional Website) Remake.

This Api utilizes .Net with Mediatr, Fluent Validation for validation, Serilog for logging and this Api have implemented standard RFC 7807 for error handling with clean architecture and MARVEL Pattern. The DMBS used is PostgreSQL.

## Asset Storage Implementation
In this Api, asset such as image, video, pdf, etc are stored in local storage located in the "App Layer" using wwwroot which serve as web root for static files. The folder location is wwwroot/upload. 
The file name is then stored along with upload directory as relative path in the Database (Ex. /uploads/news/myfile.jpg). This path is then used alongside the full url to load the static files/assets (ex. http://localhost:5007/uploads/news/myfile.jpg)

On the Database side, all static files including image, video, pdf, etc metadata and relative path are stored inside assets table which contain the path and meta data for the static files. assets table utilize polymorphic association where the model_type column will contain
a path or identification with this structure: "SourceTableName\ColumnNameInSourceTable" and the model_id column will contain the source table id. 

For example

News have a image/thumbnail and we want to call it news_image, since the table for news is called news_post. Then the model_type in assets will be news_posts\news_image with model_id referring to the id of the related news_posts row data.

Below is the model_type list

<img width="649" height="603" alt="image" src="https://github.com/user-attachments/assets/41d9a28a-f43c-476e-8639-a572dc6845c6" />



## DDL
the DDL for this api endpoint is stored in STTB.WebApiStandard.Entities\Sql\migration.txt

## **Api Endpoint**
This API contains both API for STTB profile Website and the CMS endpoint.

