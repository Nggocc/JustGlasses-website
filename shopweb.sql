use master
go
if DB_ID('shopweb') is not null
	drop database shopweb
go
create database shopweb
go
USE [shopweb]
GO


Create table [Category] (
	[category_id] Integer NOT NULL,
	[name] Nvarchar(200) NOT NULL,
Constraint [pk_Category] Primary Key  ([category_id])
) 
go
Create table [Product] (
	[product_id] Integer NOT NULL,
	[name] Nvarchar(1000) NOT NULL,
	[SKU] Varchar(100) NOT NULL,
	[description] Nvarchar(2000) NOT NULL,
	[price] Decimal(10,2) NOT NULL,
	[stock] Integer NOT NULL,
	[material] Nvarchar(100) NOT NULL,
	[gender] Nvarchar(10) NOT NULL,
	[shape] Nvarchar(100) NOT NULL,
	[color] Nvarchar(100) NOT NULL,
	[image] Char(20) NOT NULL,
	[gallery] Char(100) NOT NULL,
	[eye_width] Float NOT NULL,
	[eye_lenth] Float NOT NULL,
	[InventoryDate] Datetime NULL,
	[entryPrice] Float NULL,
	[category_id] Integer NOT NULL,
Primary Key  ([product_id])
) 
go



Create table [Cart] (
	[cart_id] Integer NOT NULL,
	[customer_id] Integer NOT NULL,
Constraint [pk_Cart] Primary Key  ([cart_id])
) 
go

Create table [Cart_Details] (
	[product_id] Integer NOT NULL,
	[cart_id] Integer NOT NULL,
	[quanlity] Integer NOT NULL,
Constraint [pk_Cart_Details] Primary Key  ([product_id],[cart_id])
) 
go

Create table [Customer] (
	[customer_id] Integer NOT NULL,
	[name] Nvarchar(100),
	[email] Char(100) NOT NULL,
	[phone] Char(11),
	[address] Nvarchar(200),
	[image] Nvarchar(200)
Constraint [pk_Customer] Primary Key  ([customer_id])
) 
go

Create table [Account] (
	[loginName] Char(30) NOT NULL,
	[password] Char(30) NOT NULL,
	[role] Nvarchar(50) NOT NULL,
	[customer_id] Integer NOT NULL ,
Constraint [pk_Account] Primary Key  ([loginName])
) 
go

Create table [Payment] (
	[payment_id] Integer NOT NULL,
	[date] Datetime NOT NULL,
	[payMethod] Nvarchar(100) NULL,
	[customer_id] Integer NOT NULL,
Constraint [pk_Payment] Primary Key  ([payment_id])
) 
go

Create table [Order] (
	[order_id] Integer NOT NULL,
	[date] Datetime NOT NULL,
	[status] Nvarchar(100) NULL,
	[shipment_id] Integer NOT NULL,
	[payment_id] Integer NOT NULL,
Constraint [pk_Order] Primary Key  ([order_id])
) 
go

Create table [Order_Details] (
	[quantity] Integer NOT NULL,
	[order_id] Integer NOT NULL,
	[product_id] Integer NOT NULL,
Constraint [pk_Order_Details] Primary Key  ([order_id],[product_id])
) 
go

Create table [Shipment] (
	[shipment_id] Integer NOT NULL,
	[address] Nvarchar(100) NOT NULL,
	[city] Nvarchar(50) NOT NULL,
	[receiverName] Nvarchar(100) NOT NULL,
	[phone] Char(11) NOT NULL,
	[customer_id] Integer NOT NULL,
Constraint [pk_Shipment] Primary Key  ([shipment_id])
) 
go

Create table [WishList] (
	[wishList_id] Integer NOT NULL,
	[customer_id] Integer NOT NULL,
	[product_id] Integer NOT NULL,
Constraint [pk_WishList] Primary Key  ([wishList_id],[customer_id],[product_id])
) 
go


Alter table [Cart_Details] add Constraint [fk_cartdetails_pro] foreign key([product_id]) references [Product] ([product_id]) 
go
Alter table [Order_Details] add Constraint [fk_orderdetails_pro] foreign key([product_id]) references [Product] ([product_id]) 
go
Alter table [WishList] add Constraint [fk_wishlist_pro] foreign key([product_id]) references [Product] ([product_id]) 
go
Alter table [Product] add Constraint [fk_pro_cat] foreign key([category_id]) references [Category] ([category_id]) 
go
Alter table [Cart_Details] add Constraint [fk_cartdetails_cart] foreign key([cart_id]) references [Cart] ([cart_id]) 
go
Alter table [Cart] add Constraint [fk_cart_customer] foreign key([customer_id]) references [Customer] ([customer_id]) 
go
Alter table [Shipment] add Constraint [fk_ship_customer] foreign key([customer_id]) references [Customer] ([customer_id]) 
go
Alter table [WishList] add Constraint [fk_wishlist_customer] foreign key([customer_id]) references [Customer] ([customer_id]) 
go
Alter table [Account] add Constraint [fk_acc_customer] foreign key([customer_id]) references [Customer] ([customer_id]) 
go
Alter table [Payment] add Constraint [fk_pay_customer] foreign key([customer_id]) references [Customer] ([customer_id]) 
go
Alter table [Order] add Constraint [fk_order_pay] foreign key([payment_id]) references [Payment] ([payment_id]) 
go
Alter table [Order_Details] add Constraint [fk_orderdetail_order] foreign key([order_id]) references [Order] ([order_id]) 
go
Alter table [Order] add Constraint [fk_order_ship] foreign key([shipment_id]) references [Shipment] ([shipment_id]) 
go


Set quoted_identifier on
go


Set quoted_identifier off
go
insert into Category values(1, N'Kính râm'), (2,N'Gọng kính cận'), (3, N'Kính râm cận'), (4, N'Tròng kính');

insert into Customer values(1, N'Lê Thị Ngọc Ánh', 'leeanh@gmail.com', '0987334567', N'Thái Bình', 'a1.jpg');
insert into Customer values(2, N'Nguyễn Thị Mai Linh', 'mlee@gmail.com', '09873345678', N'Hà Nội', 'a2.jpg');
insert into Customer values(2, N'Phạm Trọng Nghĩa', 'mlee@gmail.com', '09873345678', N'Hà Nội', 'a2.jpg');
insert into Account values('GAUCHO', '123abc', 'user', 1);
insert into Account values('GAUMEO', '123abc', 'user', 2);
insert into Account values('STupid', '12345678', 'admin', 3);


insert into Product ([product_id],[name], [SKU], [description], [price], [stock], [material], [gender], [shape], [color], [image], [gallery], [eye_width], [eye_lenth], [InventoryDate], [entryPrice], [category_id]) values
(1, N'Kính râm thời trang Reeman màu đỏ', 'KM1001', N'Kính râm thời trang RM 9080 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9080 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ,chống chịu tốt bởi tác động của môi trường, … Reeman 9080 nổi bật với sự kết hợp khéo léo gam màu mắt đỏ cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 255000, 100, N'Nhựa', N'unisex', N'Vuông', N'Đỏ','a11.jpg', 'a11.jpg,a12.jpg,a13.jpg',52.00 , 145, '2024-06-04', 180000, 1),
(2,N'Kính râm thời trang Reeman màu xanh', 'KM1002', N'Kính râm thời trang RM 9081 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9081 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9081 nổi bật với sự kết hợp khéo léo gam màu mắt xanh cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 260000, 120, N'Nhựa', N'unisex', N'Vuông', N'Xanh', 'a21.jpg', 'a21.jpg,a22.jpg,a23.jpg', 53.00, 146, '2024-06-05', 185000, 1),
(3,N'Kính râm thời trang Reeman màu đen', 'KM1003', N'Kính râm thời trang RM 9082 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9082 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9082 nổi bật với sự kết hợp khéo léo gam màu mắt đen cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 270000, 110, N'Nhựa', N'unisex', N'Vuông', N'Đen', 'a31.jpg', 'b31.jpg,b32.jpg,b33.jpg', 54.00, 147, '2024-06-06', 190000, 1),
(4,N'Kính râm thời trang Reeman màu nâu', 'KM1004', N'Kính râm thời trang RM 9083 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9083 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9083 nổi bật với sự kết hợp khéo léo gam màu mắt trắng cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 280000, 130, N'Nhựa', N'unisex', N'Vuông', N'Trắng', 'a41.jpg', 'a41.jpg,a42.jpg,a43.jpg', 55.00, 148, '2024-06-07', 195000, 1),
(5,N'KÍNH RÂM THỜI TRANG REEMAN 8157 DÁNG MẮT MÈO MÀU TÍM', 'KM1005', N'Kính râm thời trang RM 9084 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9084 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9084 nổi bật với sự kết hợp khéo léo gam màu mắt vàng cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 290000, 140, N'Nhựa', N'unisex', N'Vuông', N'Vàng', 'b11.png', 'b11.png,b12.png,b13.png', 56.00, 149, '2024-06-08', 200000, 1),
(6,N'KÍNH RÂM THỜI TRANG REEMAN 8157 DÁNG MẮT MÈO MÀU XANH', 'KM1006', N'Kính râm thời trang RM 9085 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9085 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9085 nổi bật với sự kết hợp khéo léo gam màu mắt hồng cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 300000, 150, N'Nhựa', N'unisex', N'Vuông', N'Hồng', 'b21.png', 'b21.png,b22.png,b23.png', 57.00, 150, '2024-06-09', 205000, 1),
(7,N'KÍNH RÂM THỜI TRANG REEMAN 8157 DÁNG MẮT MÈO MÀU ĐEN', 'KM1007', N'Kính râm thời trang RM 9086 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9086 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9086 nổi bật với sự kết hợp khéo léo gam màu mắt cam cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 310000, 160, N'Nhựa', N'unisex', N'Vuông', N'Cam', 'b31.png', 'b31.png,b32.png,b33.png', 58.00, 151, '2024-06-10', 210000, 1),
(8,N'GỌNG KÍNH TR MÀU HỒNG', 'KM1008', N'Kính râm thời trang RM 9087 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9087 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9087 nổi bật với sự kết hợp khéo léo gam màu mắt tím cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 320000, 170, N'Nhựa', N'unisex', N'Vuông', N'Tím', 'c11.jpg', 'c11.jpg,c12.jpg,c13.jpg', 59.00, 152, '2024-05-11', 215000, 1),
(9,N'GỌNG KÍNH TR MÀU XANH', 'KM1009', N'Kính râm thời trang RM 9088 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9088 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9088 nổi bật với sự kết hợp khéo léo gam màu mắt nâu cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 330000, 180, N'Nhựa', N'unisex', N'Vuông', N'Nâu', 'c21.jpg', 'c21.jpg,c22.jpg,c23.jpg', 60.00, 153, '2024-05-12', 220000, 2),
(10,N'GỌNG KÍNH TR TÍM', 'KM1010', N'Kính râm thời trang RM 9089 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9089 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9089 nổi bật với sự kết hợp khéo léo gam màu mắt xanh lá cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 340000, 190, N'Nhựa', N'unisex', N'Vuông', N'Xanh lá', 'c31.jpg', 'c31.jpg,c32.jpg,c33.jpg', 61.00, 154, '2024-05-13', 225000, 2),
(11,N'GỌNG KÍNH CẬN BLANCY ĐEN BÓNG VIỀN BẠC', 'KM1011', N'Kính râm thời trang RM 9090 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9090 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9090 nổi bật với sự kết hợp khéo léo gam màu mắt xanh dương cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 350000, 200, N'Nhựa', N'unisex', N'Vuông', N'Xanh dương', 'd11.jpg', 'd11.jpg,d12.jpg,d13.jpg', 62.00, 155, '2024-05-14', 230000, 2),
(12,N'GỌNG KÍNH CẬN BLANCY XÁM', 'KM1012', N'Kính râm thời trang RM 9091 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9091 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9091 nổi bật với sự kết hợp khéo léo gam màu mắt đen xám cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 360000, 210, N'Nhựa', N'unisex', N'Vuông', N'Đen xám', 'd21.jpg', 'd21.jpg,d22.jpg,d23.jpg', 63.00, 156, '2024-05-15', 235000, 2),
(13,N'GỌNG KÍNH CẬN BLANCY ĐEN NHÁM VIÊN BẠC', 'KM1013', N'Kính râm thời trang RM 9092 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9092 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9092 nổi bật với sự kết hợp khéo léo gam màu mắt bạc cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 370000, 220, N'Nhựa', N'unisex', N'Vuông', N'Bạc', 'd31.jpg', 'd31.jpg,d32.jpg,d33.jpg', 64.00, 157, '2024-05-16', 240000, 2),
(14,N'GỌNG KÍNH CẬN BLANCY ĐEN BÓNG VIÊN GHI', 'KM1014', N'Kính râm thời trang RM 9093 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9093 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9093 nổi bật với sự kết hợp khéo léo gam màu mắt xám tro cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 380000, 230, N'Nhựa', N'unisex', N'Vuông', N'Xám tro', 'd41.jpg', 'd41.jpg,d42.jpg,d43.jpg', 65.00, 158, '2024-05-17', 245000, 2),
(15,N'GỌNG KÍNH CẬN BLANCY ĐEN BÓNG VIÊN BẠC', 'KM1015', N'Kính râm thời trang RM 9094 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9094 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9094 nổi bật với sự kết hợp khéo léo gam màu mắt đỏ sẫm cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 390000, 240, N'Nhựa', N'unisex', N'Vuông', N'Đỏ sẫm', 'd51.jpg', 'd51.jpg,d52.jpg,d53.jpg', 66.00, 159, '2024-05-18', 250000, 3),
(16,N'GỌNG KÍNH CẬN BLANCY ĐỒI MỒI', 'KM1016', N'Kính râm thời trang RM 9095 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9095 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9095 nổi bật với sự kết hợp khéo léo gam màu mắt xanh lam cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 400000, 250, N'Nhựa', N'unisex', N'Vuông', N'Xanh lam', 'd61.jpg', 'd61.jpg,d62.jpg,d63.jpg', 67.00, 160, '2024-05-19', 255000, 3),
(17,N'GỌNG KÍNH RÂM CẬN 2IN1 XÁM KHÓI', 'KM1017', N'Kính râm thời trang RM 9096 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9096 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9096 nổi bật với sự kết hợp khéo léo gam màu mắt vàng nhạt cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 410000, 260, N'Nhựa', N'unisex', N'Vuông', N'Vàng nhạt', 'e11.png', 'e11.png,e12.png,e13.png', 68.00, 161, '2024-05-20', 260000, 3),
(18,N'GỌNG KÍNH RÂM CẬN 2IN2 CAM', 'KM1018', N'Kính râm thời trang RM 9097 là mẫu sản phẩm được thiết kế độc quyền bởi Ree-man. RM9097 được thiết kế bởi chất liệu nhựa cao cấp bền bỉ với thời gian, mang đến những trải nghiệm độc đáo như: giá trị sử dụng lâu dài, mặt kính bóng đẹp, khó bị gỉ, chống chịu tốt bởi tác động của môi trường, … Reeman 9097 nổi bật với sự kết hợp khéo léo gam màu mắt đỏ tươi cá tính cho cả phần gọng và mắt kính cá tính, tất cả tạo nên một tổng thể hài hoà. Chắc chắn sẽ đem lại vẻ thời trang tối đa cho bạn.', 420000, 270, N'Nhựa', N'unisex', N'Vuông', N'Đỏ tươi', 'e21.png', 'e21.png,e22.png,e23.png', 69.00, 162, '2024-05-21', 265000, 3);
go
SET IDENTITY_INSERT Product OFF;
select * from Customer;
select * from Account;
select * from Product;
select * from Category;
select * from Cart_Details;
select * from Shipment;
select * from Order_Details;
select * FROM [shopweb].[dbo].[Order]
select * from Payment;
--
