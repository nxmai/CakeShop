create database cakeshop
go

use cakeshop
go


create table category
(
	catId int not null,
	name nvarchar(100),
	amount int

	constraint pk_category primary key (catId)
)
go

create table cake
(
	cakeId int not null,
	name nvarchar(100),
	price int,
	description nvarchar(1000),
	categoryId int,
	thumbnailPath char(100)

	constraint pk_cake primary key (cakeId)
	constraint fk_cake foreign key (categoryId) references category(catId)
)
go

create table image 
(
	imgId int not null,
	path char (100),
	cakeId int

	constraint pk_img primary key(imgId)
	constraint fk_img foreign key (cakeId) references cake(cakeId)
)
go

create table cart
(
	cartId int not null,
	total int,
	createAt datetime,
	isCompleted bit

	constraint pk_cart primary key (cartId)
)
go

create table cakeInCart
(
	cicId int not null,
	cakeId int,
	amount int,
	cartId int

	constraint pk_cakeInCart primary key (cicId)
	constraint fk1 foreign key (cakeId) references cake(cakeId), 
	constraint fk2 foreign key (cartId) references cart(cartId)
)
go

insert into category values (1, 'CUPCAKE/MUFFIN', null);
insert into category values (2, N'BÁNH KEM', null);
insert into category values (3, 'CHEESE CAKE', null);
insert into category values (4, N'BÁNH MÌ', null);
insert into category values (5, 'COOKIES', null);
insert into category values (6, N'MÓN TRÁNG MIỆNG', null);
insert into category values (7, N'KHÁC', null)

insert into cake values (1, N'Sôcôla Cupcake', 30000, N'Bánh Cupcake sẽ là lựa chọn hàng đầu cho những buổi tiệc trà họp mặt như vậy bởi hương vị ngọt ngào, đặc biệt rất dễ làm ngay tại nhà', 1, 'Data/jacob-boavista-B6kBrzkl3YQ-unsplash.jpg');
insert into image values (1, 'Data/jacob-boavista-B6kBrzkl3YQ-unsplash.jpg', 1);
insert into image values (2, 'Data/photo-1582760999829-592ad50c53e3.jpg', 1);
insert into image values (3, 'Data/photo-1582760999860-bc6c933093da.jpg', 1);

insert into cake values (2, N'Vanilla Cupcake', 25000, N'Bánh Cupcake sẽ là lựa chọn hàng đầu cho những buổi tiệc trà họp mặt như vậy bởi hương vị ngọt ngào, đặc biệt rất dễ làm ngay tại nhà', 1, 'Data/photo-1486428128344-5413e434ad35.jpg');
insert into image values (4, 'Data/photo-1486428128344-5413e434ad35.jpg', 2);
insert into image values (5, 'Data/photo-1486427944299-d1955d23e34d.jpg', 2);

insert into cake values (3, N'Bánh Kem Socola', 500000, N'Bánh kem được làm bằng socola tươi và béo', 2 , 'Data/photo-1579306194873-1be32997525a.jpg');
insert into image values (6, 'Data/photo-1579306194873-1be32997525a.jpg', 3);
insert into image values (7, 'Data/photo-1579306194872-64d3b7bac4c2.jpg', 3);

insert into cake values (4, N'Bánh Kem Cho Trẻ', 350000, N'Những chiếc bánh sinh nhật ngộ nghĩnh, đáng yêu không chỉ góp phần làm bữa tiệc sinh nhật bé thêm rực rỡ mà còn làmón quà sinh nhật ý nghĩa mà ba mẹ dành tặng bé . Với các bé khi lựa chọn những mẫu bánh sinh nhật thì tiêu chí mẫu mã đẹp đáng yêu, bắt mắt luôn được đặt lên hàng đầu.', 2, N'photo-1605499502803-8109c2eec0ca.jpg');
insert into image values (8, 'Data/photo-1605499502803-8109c2eec0ca.jpg', 4);
insert into image values (9, 'Data/photo-1605499502603-e7a7866a9066.jpg', 4);
insert into image values (10, 'Data/photo-1605499502594-e30c84a55cf9.jpg', 4);

insert into cake values (5, N'Cheese Cake Chanh Dây', 320000, N'ất cả những vị chua, ngọt và béo hòa quyện một cách dễ chịu làm cho chiếc trở thành 1 trong những bánh cheesecake được yêu thích trên toàn thế giới', 3, 'Data/banh-kem-dolce-vita-mousse-chanh-day.png');
insert into image values (11, 'Data/banh-kem-dolce-vita-mousse-chanh-day.png.png', 5);
insert into image values (12, 'Data/passion-fruit-mousse-tiem-banh-dolce-vita-quan-4.png', 5);
insert into image values (13, 'Data/passion-fruit-mousse-tiem-banh-dolce-vita-quan-1.png', 5);

insert into cake values (6, N'Matcha Yogurt Mouse', 270000, N'Bánh làm từ bông lan trà xanh; lớp kem mousse gồm phô mai, sữa chua, bột trà xanh, kem tươi whipping. Màu của bánh là màu tự nhiên của trà xanh. Vì trà xanh không tan hoàn toàn trong mousse nên tạo cảm giác "nhẫn nhẫn" không phải dễ ăn, nên đây là chiếc bánh dành riêng cho tín đồ của trà xanh.', 3, 'Data/banh-kem-dolce-vita-mousse-matcha.png');
insert into image values (14, 'Data/banh-kem-dolce-vita-mousse-matcha.png', 6);
insert into image values (15, 'Data/matcha-yogurt-mousse-banh-kem-tra-xanh-sua-chua-tiem-banh-dolce-vita-quan-3.png', 6);
insert into image values (16, 'Data/matcha-yogurt-mousse-banh-kem-tra-xanh-sua-chua-tiem-banh-dolce-vita-quan-1.png', 6);

insert into cake values (7, N'Tiramisu', 420000, N'TIRAMISU là một loại bánh ngọt tráng miệng vị cà phê rất nổi tiếng của nước Ý. Bánh gồm các lớp bánh Lady Fingers (Savoiardi, [savoˈjardi] (là một loại bánh quy, giống như bánh champaign của Việt Nam nhưng ngon hơn) nhúng cà phê và rượu xen kẽ với hỗn hợp trứng, đường, phô mai mascarpone đánh bông. Vị đắng của tiramisu có nhờ lớp bột cacao phủ đều mặt bánh. ', 3, 'Data/tiramisu-mousse-cake-banh-kem-y-quan-1.png');
insert into image values (17, 'Data/tiramisu-mousse-cake-banh-kem-y-quan-1.png', 7);
insert into image values (18, 'Data/tiramisu-mousse-cake-banh-kem-y-quan-3.png', 7);
insert into image values (19, 'Data/11-442152bf-ae65-4a11-a143-5b0524187352.png', 7);
insert into image values (20, 'Data/banh-kem-dolce-vita-tiramisu-3.png', 7);

insert into cake values (8, N'Bông Lan Cuộn Phô Mai Trứng Muối', 150000, N'Bánh bông lan là một món ăn đã quá quen thuộc với đại đa số người Việt Nam. Nay để biến tấu món ăn thêm phần lạ mắt và ngon miệng, chúng ta có thêm một lựa chọn với món bánh bông lan cuộn khi kết hợp chà bông phủ ngoài và bên trong là nhân trứng muối cùng với xốt phô mai đặc trưng chắc chắn sẽ là một món ăn tuyệt vời cho những vị khách sành điệu.', 4, 'Data/(443x443)_fh_Bong_lan_cuon_Pho_mai_trung_muoi.jpg');
insert into image values (21, 'Data/(443x443)_fh_Bong_lan_cuon_Pho_mai_trung_muoi.jpg', 8);
insert into image values (22, 'Data/8zEvGi.jpg', 8);
insert into image values (23, 'Data/15032237_10154613820183950_7926599802731564217_n.jpg', 8);

insert into cake values (9, N'Bánh Mì Hoa Cúc', 80000, N'Bánh mì hoa cúc là một món bánh nổi tiếng có xuất xứ từ Pháp, được làm chủ yếu từ bột mì, bơ và vani. Tên gọi bánh mì hoa cúc là do các chị em Việt Nam nghĩ ra, bởi khi nướng xong, các vắt bánh nở bung những thớ vàng ươm trông giống như bông hoa cúc. ', 4, 'Data/banhmyhoacuc.jpg');
insert into image values (24, 'Data/banhmyhoacuc.jpg', 9);
insert into image values (25, 'Data/banh-my-hoa-cuc-330g_2019-12-02-173941.jpg', 9);
insert into image values (26, 'Data/cong-thuc-lam-banh-mi-hoa-cu.jpg', 9);

insert into cake values (10, N'Bánh Mì Cua Sữa', 15000, N'Bánh mì cua sữa (bánh mì ngọt) là món bánh có vị béo của sữa và rất thơm mùi bơ, bánh mềm xốp và được tạo dáng hình con cua nhỏ xinh nên cực kỳ thu hút các bé nhỏ. Món bánh này nhanh chóng được các bà mẹ có con nhỏ tìm hiểu khám phá để có thể tự làm ở nhà phục vụ các thiên thần nhí của mình.', 4, 'Data/banh-mi-cua-sua-002.jpg');
insert into image values (27, 'Data/banh-mi-cua-sua-002.jpg', 10);
insert into image values (28, 'Data/banh-mi-cua-sua-004.jpg', 10);
insert into image values (29, 'Data/cooky-recipe-cover-r35488.jpg' ,10);

insert into cake values (11, N'Bánh Quy Socola', 10000, N'Bánh quy sô cô la là một loại bánh quy nhỏ có các vụn sô cô la hoặc các mảnh sô cô la làm thành phần phân biệt của nó. Bánh quy sô cô la chip có nguồn gốc ở Hoa Kỳ vào khoảng năm 1938, khi Ruth Graves Wakefield cắt nhỏ một thanh sô cô la nửa ngọt của Nestlé và thêm sô cô la cắt nhỏ vào công thức bánh quy.', 5, 'Data/BAKERY-STYLE-CHOCOLATE-CHIP-COOKIES-9.jpg');
insert into image values (30, 'Data/BAKERY-STYLE-CHOCOLATE-CHIP-COOKIES-9.jpg', 11);
insert into image values (31, 'Data/Chocolate-Chip-Cookies-Recipe-720x720.jpg' ,11);
insert into image values (32, 'Data/The-Best-Chocolate-Chip-Cookies-2.jpg', 11);

insert into cake values (12, N'Cookies Dừa Hạnh Nhân', 8000, N'Bánh giòn và xốp, thơm ơi là thơm nữa, mở hộp ra là thấy ngào ngạt rùi. Bánh ngậy vị bơ, thơm mùi dừa và thêm chút bùi bùi của hạnh nhân nữa.', 5, 'Data/unnamed.jpg');
insert into image values (33, 'Data/unnamed.jpg', 12);
insert into image values (34, 'Data/6361690607_0229b7b768_z.jpg', 12);

insert into cake values (13, N'Brownie', 15000, N'Brownie là loại bánh đặc thù của chocolate. Nó là sự kết hợp hoàn hảo của bơ, bột cacao và chocolate đen. Thật lý tưởng nếu thưởng thức chiếc bánh này cùng với ly cafe trong buổi trà chiều ..Đây là sự lựa chọn mới cho những ai thích bánh sinh nhật nhưng không thích kem, không thích ngọt và là tín đồ của chocolate.', 6, 'Data/dark-chocolate-brownie-cake-dolce-vita-banh-socola-tiem-banh-quan-3.png');
insert into image values (35, 'Data/dark-chocolate-brownie-cake-dolce-vita-banh-socola-tiem-banh-quan-3.png', 13);
insert into image values (36, 'Data/dark-chocolate-brownie-cake-dolce-vita-banh-socola-tiem-banh-quan-3-2.png', 13);

insert into cake values (14, N'Donut', 10000, N'Bánh vòng là một loại bánh ngọt rán hoặc nướng để ăn tráng miệng hoặc ăn vặt. Đây là loại bánh rất nổi tiếng và phổ biến ở nhiều nước phương Tây', 7, 'Data/iStock-936102564.jpg');
insert into image values (37, 'Data/iStock-936102564.jpg', 14);
insert into image values (38, 'Data/Donuts_with_chocolate_banana_and_strawberry.jpg', 14);



select * from category;

select * from cake;

select * from cake join category on cake.categoryId = category.catId where category.catId = 1;