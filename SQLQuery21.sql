-- Bảng ADMIN
CREATE TABLE ADMIN
(
    MaAd INT IDENTITY(1,1),
    HoTen NVARCHAR(50) NOT NULL,
    DienThoai VARCHAR(15),
    TenDN VARCHAR(30) UNIQUE,
    MatKhau VARCHAR(30) NOT NULL,
    Quyen INT, -- (1: Quản trị viên, 2: Nhân viên, 3: ... - tùy theo yêu cầu cụ thể)
    CONSTRAINT PK_ADMIN PRIMARY KEY(MaAd)
)
-- Bảng loại thú cưng
CREATE TABLE LOAITHUCUNG
(
    MaLoaiTC INT IDENTITY(1,1),
    TenLoaiTC NVARCHAR(50) NOT NULL,
    MoTa NTEXT,
    CONSTRAINT PK_LOAITHUCUNG PRIMARY KEY(MaLoaiTC)
)
GO

-- Bảng nhà cung cấp thú cưng
CREATE TABLE NHACUNGCAP
(
    MaNCC INT IDENTITY(1,1),
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(150),
    DienThoai NVARCHAR(15),
    CONSTRAINT PK_NCC PRIMARY KEY(MaNCC)
)
GO

-- Bảng thú cưng
CREATE TABLE THUCUNG
(
    MaTC INT IDENTITY(1,1),
    TenTC NVARCHAR(100) NOT NULL,
    MoTa NTEXT,
    AnhBia VARCHAR(50),
    NgayCapNhat SMALLDATETIME,
    SoLuong INT CHECK(SoLuong > 0),
    GiaBan MONEY CHECK(GiaBan >= 0),
    MaLoaiTC INT,
    MaNCC INT,
    CONSTRAINT PK_TC PRIMARY KEY(MaTC)
)
GO

-- Bảng khách hàng
CREATE TABLE KHACHHANG
(
    MaKH INT IDENTITY(1,1),
    HoTen NVARCHAR(50) NOT NULL,
    TaiKhoan VARCHAR(15) UNIQUE,
    MatKhau VARCHAR(15) NOT NULL,
    Email VARCHAR(50) UNIQUE,
    DiaChi NVARCHAR(50),
    DienThoai VARCHAR(10),
    NgaySinh SMALLDATETIME,
    CONSTRAINT PK_KH PRIMARY KEY(MaKH)
)
GO

-- Bảng đơn đặt hàng
CREATE TABLE DONDATHANG
(
    MaDonHang INT IDENTITY(1,1),
    DaThanhToan BIT DEFAULT(0),
    TinhTrangGiaoHang INT DEFAULT(1),
    NgayDat SMALLDATETIME,
    NgayGiao SMALLDATETIME,
    MaKH INT,
    CONSTRAINT PK_DDH PRIMARY KEY(MaDonHang)
)
GO

-- Bảng chi tiết đặt hàng
CREATE TABLE CHITIETDATHANG
(
    MaDonHang INT,
    MaTC INT,
    SoLuong INT CHECK(SoLuong > 0),
    DonGia DECIMAL(9,2) CHECK(DonGia >= 0),
    CONSTRAINT PK_CTDH PRIMARY KEY(MaDonHang, MaTC)
)

-- Chèn dữ liệu vào bảng ADMIN
INSERT INTO ADMIN (HoTen, DienThoai, TenDN, MatKhau, Quyen)
VALUES 
    (N'Nguyễn Văn A', '0123456789', 'admin123', 'adminpass', 1),
    (N'Trần Thị B', '0987654321', 'manager456', 'managerpass', 2),
    (N'Lê Hoàng C', '0123456780', 'staff789', 'staffpass', 3);

-- Chèn dữ liệu vào bảng LOAITHUCUNG
INSERT INTO LOAITHUCUNG (TenLoaiTC, MoTa)
VALUES 
    (N'Chó', N'Thú cưng thuộc họ Canidae'),
    (N'Mèo', N'Thú cưng thuộc họ Felidae'),
    (N'Chim', N'Thú cưng có bốn chân và bộ lông');

-- Chèn dữ liệu vào bảng NHACUNGCAP
INSERT INTO NHACUNGCAP (TenNCC, DiaChi, DienThoai)
VALUES 
    (N'Công ty thú cưng A', N'Địa chỉ A', '0123456789'),
    (N'Cửa hàng thú cưng B', N'Địa chỉ B', '0987654321'),
    (N'Dịch vụ nuôi thú cưng C', N'Địa chỉ C', '0112233445');

-- Chèn dữ liệu vào bảng THUCUNG
INSERT INTO THUCUNG (TenTC, MoTa, AnhBia, NgayCapNhat, SoLuong, GiaBan, MaLoaiTC, MaNCC)
VALUES 
    (N'Beagle', N'Chó Beagle nhỏ, hoạt bát và thân thiện', 'beagle.jpg', GETDATE(), 5, 800000, 1, 1),
    (N'Mèo Ba Tư', N'Mèo Ba Tư lông dài, nổi tiếng với bộ lông mềm mại', 'persian_cat.jpg', GETDATE(), 3, 1200000, 2, 2),
    (N'Chó Husky', N'Chó Husky với bộ lông màu trắng độc đáo', 'husky.jpg', GETDATE(), 4, 1500000, 1, 1);

-- Chèn dữ liệu vào bảng KHACHHANG
INSERT INTO KHACHHANG (HoTen, TaiKhoan, MatKhau, Email, DiaChi, DienThoai, NgaySinh)
VALUES 
    (N'Nguyễn Thị Khách Hàng', 'user123', 'userpass', 'user@example.com', N'Địa chỉ khách hàng A', '0123456789', '1990-01-01'),
    (N'Trần Văn Khách Hàng', 'customer456', 'customerpass', 'customer@example.com', N'Địa chỉ khách hàng B', '0987654321', '1985-05-05'),
    (N'Lê Thị Khách Hàng', 'client789', 'clientpass', 'client@example.com', N'Địa chỉ khách hàng C', '0112233445', '2000-10-10');

-- Chèn dữ liệu vào bảng DONDATHANG
INSERT INTO DONDATHANG (DaThanhToan, TinhTrangGiaoHang, NgayDat, NgayGiao, MaKH)
VALUES 
    (0, 1, GETDATE(), NULL, 1),
    (1, 2, GETDATE(), GETDATE(), 2),
    (1, 1, GETDATE(), GETDATE(), 3);

-- Chèn dữ liệu vào bảng CHITIETDATHANG
INSERT INTO CHITIETDATHANG (MaDonHang, MaTC, SoLuong, DonGia)
VALUES 
    (1, 1, 2, 100000),
    (2, 2, 1, 150000),
    (3, 3, 3, 120000);


ALTER TABLE THUCUNG
ADD CONSTRAINT FK_THUCUNG_LOAITHUCUNG FOREIGN KEY (MaLoaiTC) 
    REFERENCES LOAITHUCUNG(MaLoaiTC);

ALTER TABLE THUCUNG
ADD CONSTRAINT FK_THUCUNG_NHACUNGCAP FOREIGN KEY (MaNCC) 
    REFERENCES NHACUNGCAP(MaNCC);
	


ALTER TABLE DONDATHANG
ADD CONSTRAINT FK_DONDATHANG_KHACHHANG FOREIGN KEY (MaKH) 
    REFERENCES KHACHHANG(MaKH);



--ALTER TABLE CHITIETDATHANG
--ADD CONSTRAINT FK_CHITIETDATHANG_DONDATHANG FOREIGN KEY (MaDonHang, MaTC) 
--    REFERENCES DONDATHANG(MaDonHang, MaTC);

--ALTER TABLE CHITIETDATHANG
--ADD CONSTRAINT FK_CHITIETDATHANG_THUCUNG FOREIGN KEY (MaTC) 
--    REFERENCES THUCUNG(MaTC);




--ALTER TABLE CHITIETDATHANG
--DROP CONSTRAINT PK_CTDH; -- Xóa ràng buộc khoá chính

--ALTER TABLE CHITIETDATHANG
--ADD CONSTRAINT FK_CHITIETDATHANG_THUCUNG FOREIGN KEY (MaTC) 
--    REFERENCES THUCUNG(MaTC);

--ALTER TABLE CHITIETDATHANG
--ADD CONSTRAINT FK_CHITIETDATHANG_THUCUNG FOREIGN KEY (MaTC) 
--    REFERENCES THUCUNG(MaTC);

--	select * from THUCUNG

--	ALTER TABLE CHITIETDATHANG
--ADD CONSTRAINT PK_CHITIETDATHANG PRIMARY KEY (MaDonHang, MaTC); -- Assuming a composite primary key

--ALTER TABLE DONDATHANG
--ADD CONSTRAINT FK_DONDATHANG_CHITIETDATHANG FOREIGN KEY (MaDonHang) 
--    REFERENCES CHITIETDATHANG(MaDonHang);

--ALTER TABLE THUCUNG
--ADD CONSTRAINT FK_THUCUNG_CHITIETDATHANG FOREIGN KEY (MaTC) 
--    REFERENCES CHITIETDATHANG(MaTC);





	-- Bảng thông tin thanh toán
CREATE TABLE THANHTOAN
(
    MaThanhToan INT IDENTITY(1,1),
    MaDonHang INT,
    MaKH INT,
    PhuongThucThanhToan NVARCHAR(50), -- Ví dụ: Thẻ tín dụng, chuyển khoản, tiền mặt, etc.
    NgayThanhToan SMALLDATETIME,
    TongTienThanhToan MONEY,
    TrangThaiThanhToan INT, -- 1: Đã thanh toán, 0: Chưa thanh toán
    CONSTRAINT PK_THANHTOAN PRIMARY KEY(MaThanhToan),
    CONSTRAINT FK_THANHTOAN_DONDATHANG FOREIGN KEY(MaDonHang) REFERENCES DONDATHANG(MaDonHang),
    CONSTRAINT FK_THANHTOAN_KHACHHANG FOREIGN KEY(MaKH) REFERENCES KHACHHANG(MaKH)
)


-- Assuming you want to store messages
CREATE TABLE Messages
(
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15) NOT NULL,
    MessageText NTEXT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
)



CREATE TABLE Comment
(
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    MessageText NTEXT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
)