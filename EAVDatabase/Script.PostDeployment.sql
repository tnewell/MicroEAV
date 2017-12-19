/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
use MicroEAV

-- Reference Data for Data_Type 
MERGE INTO Data_Type AS Target 
USING (VALUES 
  (1, N'String'), 
  (2, N'Boolean'), 
  (3, N'Integer'), 
  (4, N'Float'), 
  (5, N'DateTime') 
) 
AS Source (Data_Type_ID, Name) 
ON Target.Data_Type_ID = Source.Data_Type_ID 

-- update matched rows 
WHEN MATCHED THEN UPDATE SET Name = Source.Name 

-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN INSERT (Name) VALUES (Name) 

-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN DELETE;


use MicroEAV_Test

-- Reference Data for Data_Type 
MERGE INTO Data_Type AS Target 
USING (VALUES 
  (1, N'String'), 
  (2, N'Boolean'), 
  (3, N'Integer'), 
  (4, N'Float'), 
  (5, N'DateTime') 
) 
AS Source (Data_Type_ID, Name) 
ON Target.Data_Type_ID = Source.Data_Type_ID 

-- update matched rows 
WHEN MATCHED THEN UPDATE SET Name = Source.Name 

-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN INSERT (Name) VALUES (Name) 

-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN DELETE;
