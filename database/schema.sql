-- ============================================================
-- ฐานข้อมูล: product_db (ระบบจัดการสินค้าและคำสั่งซื้อ)
-- ออกแบบสำหรับ: ข้อ 4.1
-- ตารางทั้งหมด: 4 ตาราง
-- PK ทุกตารางเป็น AUTO_INCREMENT (running number)
-- ============================================================

CREATE DATABASE IF NOT EXISTS product_db
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE product_db;

-- ------------------------------------------------------------
-- 1. ตาราง members (สมาชิก/ลูกค้า)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS members (
  member_id    INT AUTO_INCREMENT PRIMARY KEY,
  first_name   VARCHAR(50)  NOT NULL,
  last_name    VARCHAR(50)  NOT NULL,
  email        VARCHAR(100) NOT NULL UNIQUE,
  phone        VARCHAR(20)  NULL,
  created_at   TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- 2. ตาราง products (สินค้า)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS products (
  product_id   INT AUTO_INCREMENT PRIMARY KEY,
  product_name VARCHAR(100) NOT NULL,
  price        DECIMAL(10,2) NOT NULL,
  stock        INT          NOT NULL DEFAULT 0,
  category     VARCHAR(50)  NULL,
  created_at   TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- 3. ตาราง orders (คำสั่งซื้อ)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS orders (
  order_id     INT AUTO_INCREMENT PRIMARY KEY,
  member_id    INT          NOT NULL,
  order_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  total_amount DECIMAL(12,2) NOT NULL DEFAULT 0.00,
  status       ENUM('pending','paid','shipped','cancelled') DEFAULT 'pending',
  CONSTRAINT fk_orders_member FOREIGN KEY (member_id) REFERENCES members(member_id)
) ENGINE=InnoDB;

-- ------------------------------------------------------------
-- 4. ตาราง order_items (รายการสินค้าในคำสั่งซื้อ)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS order_items (
  item_id      INT AUTO_INCREMENT PRIMARY KEY,
  order_id     INT          NOT NULL,
  product_id   INT          NOT NULL,
  quantity     INT          NOT NULL DEFAULT 1,
  unit_price   DECIMAL(10,2) NOT NULL,
  CONSTRAINT fk_items_order   FOREIGN KEY (order_id)   REFERENCES orders(order_id),
  CONSTRAINT fk_items_product FOREIGN KEY (product_id) REFERENCES products(product_id)
) ENGINE=InnoDB;
