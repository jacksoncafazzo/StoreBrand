# StoreBrand: Which Stores Have Which Brands?
##### Epicodus csharp Friday Code Review Week 4

#### By Jackson Cafazzo


## Description

StoreBrand allows user to add brands and associate which stores carry them and provide modifications to the list.

## Setup

Clone repository from GitHub.
Navigate to directory.
run "dnu restore"
run "dnx kestrel"
Navigate to http://localhost:5004

###Importing in  using sqlcmd
Run sqlcmd in powershell:
sqlcmd -S "(localdb)\mssqllocaldb"
enter these commands into the sqlcmd prompt:
CREATE DATABASE shoe_stores
GO

USE shoe_stores
GO

CREATE TABLE stores (
  id INT IDENTITY(1,1),
  name VARCHAR(255),
  url VARCHAR(255)
);

CREATE TABLE brands (
  id INT IDENTITY(1,1),
  name VARCHAR(255),
  logo VARCHAR(255)
);

CREATE TABLE brands_stores (
  id INT IDENTITY(1,1),
  store_id INT,
  brand_id INT
);

###Importing databases with SSMS
Open SSMS
Select File > Open > File and select the shoe_storesDBscript.sql file.
If the database does not already exist, add the following lines to the top of the script file
CREATE DATABASE [shoe_stores]
GO
Save the file.
Click ! Execute.
Verify that the database has been created and the schema and/or data imported.
Repeat using shoe_stores_test as the database to create.

To add brand logos, first copy the brand image to the Content/img folder and then type the name of the image file in the "brand logo" form as you add a new brand.
I've added "simple.png" to the folder but no simple brand. Try it out when you add a new brand!

## Technologies Used

MSSQLlocaldb with SQL server application of your choice, .NET dnx, Razor, CRUD functionality, HTML5, CSS3, Bootstrap

### Legal

Copyright (c) 2016, Jackson Cafazzo

This software is licensed under the MIT license.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
