CREATE TABLE "DBA"."Customer" (
	"ID" uniqueidentifier NOT NULL,
	"FirstName" nvarchar(50) NULL,
	"LastName" nvarchar(50) NULL,
	"Height" numeric(18,2) NULL,
	"Weight" integer NULL,
	"BirthDay" date NULL,
	PRIMARY KEY ( "ID" ASC )
);

CREATE TABLE "DBA"."Product" (
	"ID" integer NOT NULL,
	"Code" varchar(100) NULL,
	"Name" varchar(100) NULL,
	"Manufacturer" varchar(100) NULL,
	"Size" varchar(100) NULL,
	"Discontinued" bit NULL,
	"BarCode" varchar(100) NULL,
	PRIMARY KEY ( "ID" ASC )
);


CREATE PROCEDURE "DBA"."WithOutParams"( @out1 int output,
	@out2 int output  )
AS
BEGIN
	SET @out1 = 1
	SET @out2 = 2
END