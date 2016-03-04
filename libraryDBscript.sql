
USE storebrand
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

CREATE TABLE store_brand (
  id INT IDENTITY(1,1),
  store_id INT,
  brand_id INT
);
