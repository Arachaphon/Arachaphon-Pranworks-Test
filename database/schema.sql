-- ============================================================
-- ฐานข้อมูล: shop_db (ระบบจัดการสมาชิก สินค้า และคำสั่งซื้อ)
-- ออกแบบสำหรับ: ข้อ 4.1
-- จำนวนตาราง: 3 ตาราง
-- PK ทุกตาราง: AUTO_INCREMENT (running number)
-- FK เชื่อมทุกตาราง: orders -> members, orders -> products
-- ทุกตารางมี >= 5 columns
-- Data types ครบ: INT, NVARCHAR, BOOLEAN, DATETIME
-- ============================================================

CREATE DATABASE IF NOT EXISTS shop_db
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE shop_db;

-- ------------------------------------------------------------
-- 1. ตาราง members (ข้อมูลสมาชิก)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS members (
  member_id    INT           AUTO_INCREMENT PRIMARY KEY,
  first_name   NVARCHAR(50)  NOT NULL,
  last_name    NVARCHAR(50)  NOT NULL,
  email        NVARCHAR(100) NOT NULL UNIQUE,
  phone        NVARCHAR(20)  ,
  is_active    BOOLEAN       DEFAULT TRUE,
  created_at   DATETIME      DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- 2. ตาราง products (ข้อมูลสินค้า)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS products (
  product_id   INT            AUTO_INCREMENT PRIMARY KEY,
  product_name NVARCHAR(100)  NOT NULL,
  description  NVARCHAR(500)  NULL,
  price        DECIMAL(10,2)  NOT NULL,
  stock        INT            NOT NULL DEFAULT 0,
  is_available BOOLEAN        DEFAULT TRUE,
  created_at   DATETIME       DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- 3. ตาราง orders (ข้อมูลคำสั่งซื้อ — FK เชื่อม members และ products)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS orders (
  order_id         INT            AUTO_INCREMENT PRIMARY KEY,
  member_id        INT            NOT NULL,
  product_id       INT            NOT NULL,
  quantity         INT            NOT NULL DEFAULT 1,
  shipping_address NVARCHAR(255)  NOT NULL,
  total_amount     DECIMAL(12,2)  NOT NULL DEFAULT 0.00,
  is_paid          BOOLEAN        DEFAULT FALSE,
  order_date       DATETIME       DEFAULT CURRENT_TIMESTAMP,

  CONSTRAINT fk_orders_member   FOREIGN KEY (member_id)
    REFERENCES members(member_id),
  CONSTRAINT fk_orders_product  FOREIGN KEY (product_id)
    REFERENCES products(product_id)
) ENGINE=InnoDB;
