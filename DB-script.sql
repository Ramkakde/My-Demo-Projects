USE [ContactManagement]
GO
/****** Object:  Table [dbo].[ContactDetails]    Script Date: 8/8/2018 10:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactDetails](
	[ContactDetailUID] [uniqueidentifier] NOT NULL,
	[First] [varchar](150) NULL,
	[Middle] [varchar](150) NULL,
	[Last] [varchar](150) NULL,
	[Email] [varchar](50) NULL,
	[PhoneNo] [varchar](10) NULL,
	[InactiveStatus] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactDetailUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ContactDetails] ADD  DEFAULT ((0)) FOR [InactiveStatus]
GO
/****** Object:  StoredProcedure [dbo].[Usp_AddContactDetails]    Script Date: 8/8/2018 10:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Usp_AddContactDetails]
--Declare 
@First Varchar(150),
@Middle Varchar(150),
@Last Varchar(150),
@Email Varchar(50),
@PhoneNo Varchar(10)          
AS 
BEGIN 
SELECT ContactDetailUID,First,Middle,Last ,Email ,PhoneNo 
FROM  ContactDetails 
Where 
1 =  CASE WHEN @First IS NULL THEN 1 ELSE CASE WHEN First LIKE ISNULL(@First, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @Middle IS NULL THEN 1 ELSE CASE WHEN Middle LIKE ISNULL(@Middle, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @Last IS NULL THEN 1 ELSE CASE WHEN First LIKE ISNULL(@Last, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @PhoneNo IS NULL THEN 1 ELSE CASE WHEN PhoneNo LIKE ISNULL(@PhoneNo, '')  + '%' THEN 1 ELSE 0 END END
 
END 

--AND 1 = (CASE WHEN (@MeasureSetUid IS NULL) THEN 1 ELSE (CASE WHEN(MD.NationalProgramQualityMeasureUid=vS.NationalProgramQualityMeasureUid AND MD.MeasureSetUid=@MeasureSetUid) THEN 1 ELSE 0 END) END)
	
GO
/****** Object:  StoredProcedure [dbo].[Usp_AddUpdateContactDetails]    Script Date: 8/8/2018 10:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc [dbo].[Usp_AddUpdateContactDetails]
@ContactDetailUID UNIQUEIDENTIFIER=NULL,
@First Varchar(150),
@Middle Varchar(150),
@Last Varchar(150),
@Email Varchar(50),
@PhoneNo Varchar(10),          
@InactiveStatus bit
 AS 
BEGIN 
 

IF NOT EXISTS(SELECT * FROM ContactDetails where ContactDetailUID=@ContactDetailUID)
  BEGIN 
    INSERT INTO ContactDetails(ContactDetailUID,First,Middle,Last ,Email ,PhoneNo,InactiveStatus) 
    VALUES(NEWID(),@First,@Middle,@Last ,@Email ,@PhoneNo,@InactiveStatus)
  END 
ELSE 
  BEGIN 
   UPDATE ContactDetails
	   SET 
		   First=@First,
		   Middle=@Middle,
		   Last=@Last ,
		   Email=@Email,
		   PhoneNo=@PhoneNo,
		   InactiveStatus=@InactiveStatus 
	WHERE ContactDetailUID=@ContactDetailUID
  END 
END 



 
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetContactDetails]    Script Date: 8/8/2018 10:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Usp_GetContactDetails]
--Declare 
@ContactDetailUID UNIQUEIDENTIFIER=NULL ,
@First Varchar(150)=NULL,
@Middle Varchar(150)=NULL,
@Last Varchar(150)=NULL,
@Email Varchar(50)=NULL,
@PhoneNo Varchar(10)  =NULL,
@InactiveStatus Bit 
AS 
BEGIN 
SELECT ContactDetailUID,First,Middle,Last ,Email ,PhoneNo,ISNULL(InactiveStatus,0)InactiveStatus 
FROM  ContactDetails InactiveStatus 
Where 
1 =  CASE WHEN @First IS NULL THEN 1 ELSE CASE WHEN First LIKE ISNULL(@First, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @Middle IS NULL THEN 1 ELSE CASE WHEN Middle LIKE ISNULL(@Middle, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @Last IS NULL THEN 1 ELSE CASE WHEN First LIKE ISNULL(@Last, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @PhoneNo IS NULL THEN 1 ELSE CASE WHEN PhoneNo LIKE ISNULL(@PhoneNo, '')  + '%' THEN 1 ELSE 0 END END
AND 1 =  CASE WHEN @ContactDetailUID IS NULL THEN 1 ELSE CASE WHEN ContactDetailUID =@ContactDetailUID THEN 1 ELSE 0 END END 
END 

 
GO
 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Usp_DeleteContactDetails]
@ContactDetailUID UNIQUEIDENTIFIER=NULL 
 AS 
BEGIN 
 

IF EXISTS(SELECT * FROM ContactDetails where ContactDetailUID=@ContactDetailUID)
  BEGIN 
    DELETE 
	FROM  ContactDetails 
	WHERE ContactDetailUID=@ContactDetailUID     
  END 
END 



 
