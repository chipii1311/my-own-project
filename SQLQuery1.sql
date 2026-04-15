CREATE Database [Database]
GO
USE [Database]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetDiscountAmount]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- fn_GetDiscountAmount - Tính tiền giảm
CREATE FUNCTION [dbo].[fn_GetDiscountAmount](@OrderID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @SubTotal DECIMAL(10,2) = 0;
    DECLARE @Discount DECIMAL(10,2) = 0;
    
    SELECT @SubTotal = ISNULL(SUM(SubTotal), 0) 
    FROM [dbo].[OrderDetail] 
    WHERE OrderID = @OrderID;
    
    SELECT @Discount = ISNULL(SUM(@SubTotal * p.DiscountPercent / 100), 0)
    FROM [dbo].[Orders] o
    LEFT JOIN [dbo].[Promotion] p ON o.PromotionID = p.PromotionID
    WHERE o.OrderID = @OrderID;
    
    RETURN @Discount;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetStockStatus]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- fn_GetStockStatus - Kiểm tra tồn kho
CREATE FUNCTION [dbo].[fn_GetStockStatus](@IngredientID INT)
RETURNS NVARCHAR(20)
AS
BEGIN
    DECLARE @Status NVARCHAR(20) = 'Normal';
    DECLARE @Stock FLOAT;
    DECLARE @MinStock FLOAT;
    
    SELECT @Stock = StockQuantity, @MinStock = MinStock 
    FROM [dbo].[Ingredient] 
    WHERE IngredientID = @IngredientID;
    
    IF @Stock <= 0
        SET @Status = 'OutOfStock';
    ELSE IF @Stock < @MinStock
        SET @Status = 'LowStock';
    
    RETURN @Status;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetTotalAmount]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================
-- FUNCTIONS
-- ============================================

-- fn_GetTotalAmount - Tính tổng tiền order
CREATE FUNCTION [dbo].[fn_GetTotalAmount](@OrderID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @SubTotal DECIMAL(10,2) = 0;
    DECLARE @Discount DECIMAL(10,2) = 0;
    
    SELECT @SubTotal = ISNULL(SUM(SubTotal), 0) 
    FROM [dbo].[OrderDetail] 
    WHERE OrderID = @OrderID;
    
    SELECT @Discount = ISNULL(SUM(@SubTotal * p.DiscountPercent / 100), 0)
    FROM [dbo].[Orders] o
    LEFT JOIN [dbo].[Promotion] p ON o.PromotionID = p.PromotionID
    WHERE o.OrderID = @OrderID;
    
    RETURN @SubTotal - @Discount;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_IsPromotionValid]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- fn_IsPromotionValid - Kiểm tra promotion còn hiệu lực
CREATE FUNCTION [dbo].[fn_IsPromotionValid](@PromotionID INT)
RETURNS BIT
AS
BEGIN
    DECLARE @IsValid BIT = 0;
    
    IF EXISTS (
        SELECT 1 FROM [dbo].[Promotion]
        WHERE PromotionID = @PromotionID 
        AND Status = 'Active'
        AND GETDATE() BETWEEN StartDate AND EndDate
    )
        SET @IsValid = 1;
    
    RETURN @IsValid;
END;
GO
/****** Object:  Table [dbo].[Category]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiningTable]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiningTable](
	[TableID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantID] [int] NULL,
	[TableNumber] [int] NULL,
	[Capacity] [int] NULL,
	[Status] [nvarchar](20) NULL,
	[Notes] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[TableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[FeedbackID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[UserID] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[FeedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredient]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
	[IngredientID] [int] IDENTITY(1,1) NOT NULL,
	[IngredientName] [nvarchar](100) NULL,
	[Unit] [nvarchar](20) NULL,
	[StockQuantity] [float] NULL,
	[IsActive] [bit] NULL,
	[MinStock] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[IngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryTransaction]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryTransaction](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[IngredientID] [int] NOT NULL,
	[QuantityChanged] [float] NOT NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[TransactionDate] [datetime] NULL,
	[StaffID] [int] NOT NULL,
	[Note] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuItem]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItem](
	[MenuItemID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantID] [int] NULL,
	[CategoryID] [int] NULL,
	[ItemName] [nvarchar](100) NULL,
	[Description] [nvarchar](255) NULL,
	[Price] [decimal](10, 2) NULL,
	[Status] [nvarchar](20) NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[IsAvailable] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MenuItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[MenuItemID] [int] NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](10, 2) NULL,
	[SubTotal] [decimal](10, 2) NULL,
	[Note] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderHistory]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderHistory](
	[HistoryID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[OldStatus] [nvarchar](20) NULL,
	[NewStatus] [nvarchar](20) NULL,
	[ChangedAt] [datetime] NULL,
	[ChangedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[RestaurantID] [int] NULL,
	[TableID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[OrderType] [nvarchar](20) NULL,
	[Status] [nvarchar](20) NULL,
	[TotalAmount] [decimal](10, 2) NULL,
	[UpdatedAt] [datetime] NULL,
	[StaffID] [int] NULL,
	[PromotionID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[Method] [nvarchar](50) NULL,
	[Amount] [decimal](10, 2) NULL,
	[PaymentTime] [datetime] NULL,
	[Status] [nvarchar](20) NULL,
	[TransactionID] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotion]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[PromotionID] [int] IDENTITY(1,1) NOT NULL,
	[PromotionName] [nvarchar](100) NULL,
	[DiscountPercent] [decimal](5, 2) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[PromotionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PromotionDetail]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromotionDetail](
	[PromotionDetailID] [int] IDENTITY(1,1) NOT NULL,
	[PromotionID] [int] NULL,
	[MenuItemID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PromotionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipe]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipe](
	[RecipeID] [int] IDENTITY(1,1) NOT NULL,
	[MenuItemID] [int] NULL,
	[IngredientID] [int] NULL,
	[Quantity] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[RecipeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurant]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantName] [nvarchar](100) NULL,
	[Address] [nvarchar](200) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[RestaurantID] [int] NULL,
	[Position] [nvarchar](50) NULL,
	[Salary] [decimal](10, 2) NULL,
	[HireDate] [datetime] NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[PasswordHash] [nvarchar](255) NULL,
	[Role] [nvarchar](20) NULL,
	[CreatedAt] [datetime] NULL,
	[IsActive] [bit] NULL,
	[LastLogin] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryID], [CategoryName], [IsActive]) VALUES (1, N'Món khai vị', 1)
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [IsActive]) VALUES (2, N'Món chính', 1)
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [IsActive]) VALUES (3, N'Đồ uống', 1)
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [IsActive]) VALUES (4, N'Tráng miệng', 1)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[DiningTable] ON 

INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (1, 1, 1, 2, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (2, 1, 2, 4, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (3, 1, 3, 4, N'Đang dùng', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (4, 1, 4, 6, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (5, 1, 5, 8, N'Đã đặt', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (6, 2, 1, 2, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (7, 2, 2, 4, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (8, 2, 3, 6, N'Đang dùng', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (9, 1, 1, 2, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (10, 1, 2, 4, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (11, 1, 3, 4, N'Đang dùng', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (12, 1, 4, 6, N'Có khách', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (13, 1, 5, 8, N'Đã đặt', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (14, 2, 1, 2, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (15, 2, 2, 4, N'Trống', NULL)
INSERT [dbo].[DiningTable] ([TableID], [RestaurantID], [TableNumber], [Capacity], [Status], [Notes]) VALUES (16, 2, 3, 6, N'Đang dùng', NULL)
SET IDENTITY_INSERT [dbo].[DiningTable] OFF
GO
SET IDENTITY_INSERT [dbo].[Feedback] ON 

INSERT [dbo].[Feedback] ([FeedbackID], [OrderID], [UserID], [Rating], [Comment], [CreatedAt]) VALUES (7, 1, 2, 5, N'Món ăn ngon, phục vụ tốt!', CAST(N'2024-03-20T12:15:00.000' AS DateTime))
INSERT [dbo].[Feedback] ([FeedbackID], [OrderID], [UserID], [Rating], [Comment], [CreatedAt]) VALUES (8, 3, 3, 4, N'Cơm chiên hơi mặn, nhưng nước cam ngon.', CAST(N'2024-03-21T13:00:00.000' AS DateTime))
INSERT [dbo].[Feedback] ([FeedbackID], [OrderID], [UserID], [Rating], [Comment], [CreatedAt]) VALUES (9, 1, 2, 5, N'Món ăn ngon, phục vụ tốt!', CAST(N'2024-03-20T12:15:00.000' AS DateTime))
INSERT [dbo].[Feedback] ([FeedbackID], [OrderID], [UserID], [Rating], [Comment], [CreatedAt]) VALUES (10, 3, 3, 4, N'Cơm chiên hơi mặn, nhưng nước cam ngon.', CAST(N'2024-03-21T13:00:00.000' AS DateTime))
INSERT [dbo].[Feedback] ([FeedbackID], [OrderID], [UserID], [Rating], [Comment], [CreatedAt]) VALUES (11, 1, NULL, 1, N'123', CAST(N'2026-04-01T08:25:43.897' AS DateTime))
SET IDENTITY_INSERT [dbo].[Feedback] OFF
GO
SET IDENTITY_INSERT [dbo].[Ingredient] ON 

INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (1, N'Thịt bò', N'kg', 20, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (2, N'Gà nguyên con', N'kg', 15, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (3, N'Rau xà lách', N'kg', 5, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (4, N'Cà chua', N'kg', 8, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (5, N'Coca Cola', N'lon', 100, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (6, N'Bia Tiger', N'lon', 80, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (7, N'Kem', N'hộp', 10, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (8, N'Thịt bò', N'kg', 20, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (9, N'Gà nguyên con', N'kg', 15, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (10, N'Rau xà lách', N'kg', 5, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (11, N'Cà chua', N'kg', 8, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (12, N'Coca Cola', N'lon', 100, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (13, N'Bia Tiger', N'lon', 80, NULL, NULL)
INSERT [dbo].[Ingredient] ([IngredientID], [IngredientName], [Unit], [StockQuantity], [IsActive], [MinStock]) VALUES (14, N'Kem', N'hộp', 10, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Ingredient] OFF
GO
SET IDENTITY_INSERT [dbo].[MenuItem] ON 

INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (1, 1, 1, N'Salad trộn', N'Rau củ tươi trộn sốt mayonnaise', CAST(45000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (2, 1, 1, N'Súp cua', N'Súp cua đồng, nấm hương', CAST(35000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (3, 1, 2, N'Bò bít tết', N'Bò Úc, sốt tiêu đen', CAST(120000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (4, 1, 2, N'Gà nướng mật ong', N'Gà ta nướng, mật ong rừng', CAST(150000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (5, 1, 3, N'Coca Cola', N'Nước giải khát có gas', CAST(15000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (6, 1, 3, N'Bia Tiger', N'Bia lon Tiger', CAST(20000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (7, 1, 4, N'Kem dâu', N'Kem vani sốt dâu', CAST(25000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (8, 1, 4, N'Bánh flan', N'Bánh flan caramel', CAST(20000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (9, 2, 2, N'Cơm chiên Dương Châu', N'Cơm chiên hải sản', CAST(55000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (10, 2, 3, N'Nước cam ép', N'Cam tươi nguyên chất', CAST(30000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (11, 1, 1, N'Salad trộn', N'Rau củ tươi trộn sốt mayonnaise', CAST(45000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (12, 1, 1, N'Súp cua', N'Súp cua đồng, nấm hương', CAST(35000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (13, 1, 2, N'Bò bít tết', N'Bò Úc, sốt tiêu đen', CAST(120000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (14, 1, 2, N'Gà nướng mật ong', N'Gà ta nướng, mật ong rừng', CAST(150000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (15, 1, 3, N'Coca Cola', N'Nước giải khát có gas', CAST(15000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (16, 1, 3, N'Bia Tiger', N'Bia lon Tiger', CAST(20000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (17, 1, 4, N'Kem dâu', N'Kem vani sốt dâu', CAST(25000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (18, 1, 4, N'Bánh flan', N'Bánh flan caramel', CAST(20000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (19, 2, 2, N'Cơm chiên Dương Châu', N'Cơm chiên hải sản', CAST(55000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (20, 2, 3, N'Nước cam ép', N'Cam tươi nguyên chất', CAST(30000.00 AS Decimal(10, 2)), N'Còn', NULL, 1, NULL, NULL)
INSERT [dbo].[MenuItem] ([MenuItemID], [RestaurantID], [CategoryID], [ItemName], [Description], [Price], [Status], [ImageUrl], [IsAvailable], [CreatedAt], [UpdatedAt]) VALUES (21, NULL, 2, N'Salad', NULL, CAST(4555000.00 AS Decimal(10, 2)), NULL, NULL, 0, CAST(N'2026-04-01T08:52:50.497' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[MenuItem] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 

INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (15, 1, 3, 1, CAST(120000.00 AS Decimal(10, 2)), CAST(120000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (16, 1, 5, 2, CAST(15000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (17, 1, 7, 1, CAST(25000.00 AS Decimal(10, 2)), CAST(25000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (18, 1, 1, 1, CAST(45000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (19, 2, 2, 1, CAST(35000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (20, 2, 4, 1, CAST(150000.00 AS Decimal(10, 2)), CAST(150000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (21, 2, 6, 2, CAST(20000.00 AS Decimal(10, 2)), CAST(40000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (22, 3, 9, 1, CAST(55000.00 AS Decimal(10, 2)), CAST(55000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (23, 3, 10, 1, CAST(30000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (24, 1, 3, 1, CAST(120000.00 AS Decimal(10, 2)), CAST(120000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (25, 1, 5, 2, CAST(15000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (26, 1, 7, 1, CAST(25000.00 AS Decimal(10, 2)), CAST(25000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (27, 1, 1, 1, CAST(45000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (28, 2, 2, 1, CAST(35000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (29, 2, 4, 1, CAST(150000.00 AS Decimal(10, 2)), CAST(150000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (30, 2, 6, 2, CAST(20000.00 AS Decimal(10, 2)), CAST(40000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (31, 3, 9, 1, CAST(55000.00 AS Decimal(10, 2)), CAST(55000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (32, 3, 10, 1, CAST(30000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (33, 7, 1, 1, CAST(45000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (34, 7, 2, 1, CAST(35000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (35, 7, 5, 2, CAST(15000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (36, 8, 2, 1, CAST(35000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (37, 8, 3, 1, CAST(120000.00 AS Decimal(10, 2)), CAST(120000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (38, 9, 1, 1, CAST(45000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (39, 10, 1, 1, CAST(45000.00 AS Decimal(10, 2)), CAST(45000.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[OrderDetail] ([OrderDetailID], [OrderID], [MenuItemID], [Quantity], [UnitPrice], [SubTotal], [Note]) VALUES (40, 10, 2, 1, CAST(35000.00 AS Decimal(10, 2)), CAST(35000.00 AS Decimal(10, 2)), NULL)
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (1, 2, 1, 3, CAST(N'2024-03-20T11:30:00.000' AS DateTime), N'Tại chỗ', N'Đã thanh toán', CAST(440000.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (2, 2, 1, 3, CAST(N'2024-03-20T18:45:00.000' AS DateTime), N'Tại chỗ', N'Đang dùng', CAST(450000.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (3, 3, 2, 3, CAST(N'2024-03-21T12:00:00.000' AS DateTime), N'Tại chỗ', N'Đã thanh toán', CAST(170000.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (4, 2, 1, 3, CAST(N'2024-03-20T11:30:00.000' AS DateTime), N'Tại chỗ', N'Đã thanh toán', CAST(195000.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (5, 2, 1, 3, CAST(N'2024-03-20T18:45:00.000' AS DateTime), N'Tại chỗ', N'Đang dùng', CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (6, 3, 2, 3, CAST(N'2024-03-21T12:00:00.000' AS DateTime), N'Tại chỗ', N'Đã thanh toán', CAST(85000.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (7, NULL, NULL, 1, CAST(N'2026-04-01T04:42:14.810' AS DateTime), NULL, N'Đã thanh toán', CAST(110000.00 AS Decimal(10, 2)), CAST(N'2026-04-01T04:42:15.010' AS DateTime), NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (8, NULL, NULL, 1, CAST(N'2026-04-01T08:02:40.003' AS DateTime), NULL, N'Đã thanh toán', CAST(155000.00 AS Decimal(10, 2)), CAST(N'2026-04-01T08:02:40.370' AS DateTime), NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (9, NULL, NULL, 12, CAST(N'2026-04-01T08:51:33.430' AS DateTime), NULL, N'Chưa thanh toán', CAST(45000.00 AS Decimal(10, 2)), CAST(N'2026-04-01T08:51:33.733' AS DateTime), NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [RestaurantID], [TableID], [OrderDate], [OrderType], [Status], [TotalAmount], [UpdatedAt], [StaffID], [PromotionID]) VALUES (10, NULL, NULL, 14, CAST(N'2026-04-01T09:22:56.233' AS DateTime), NULL, N'Đã thanh toán', CAST(80000.00 AS Decimal(10, 2)), CAST(N'2026-04-01T09:22:56.463' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([PaymentID], [OrderID], [Method], [Amount], [PaymentTime], [Status], [TransactionID]) VALUES (6, 1, N'Tiền mặt', CAST(195000.00 AS Decimal(10, 2)), CAST(N'2024-03-20T12:10:00.000' AS DateTime), N'Thành công', NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [Method], [Amount], [PaymentTime], [Status], [TransactionID]) VALUES (7, 3, N'Chuyển khoản', CAST(85000.00 AS Decimal(10, 2)), CAST(N'2024-03-21T12:30:00.000' AS DateTime), N'Thành công', NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [Method], [Amount], [PaymentTime], [Status], [TransactionID]) VALUES (8, 1, N'Tiền mặt', CAST(195000.00 AS Decimal(10, 2)), CAST(N'2024-03-20T12:10:00.000' AS DateTime), N'Thành công', NULL)
INSERT [dbo].[Payment] ([PaymentID], [OrderID], [Method], [Amount], [PaymentTime], [Status], [TransactionID]) VALUES (9, 3, N'Chuyển khoản', CAST(85000.00 AS Decimal(10, 2)), CAST(N'2024-03-21T12:30:00.000' AS DateTime), N'Thành công', NULL)
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[Recipe] ON 

INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (1, 3, 1, 0.2)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (2, 4, 2, 1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (3, 1, 3, 0.1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (4, 5, 5, 1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (5, 6, 6, 1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (6, 3, 1, 0.2)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (7, 4, 2, 1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (8, 1, 3, 0.1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (9, 5, 5, 1)
INSERT [dbo].[Recipe] ([RecipeID], [MenuItemID], [IngredientID], [Quantity]) VALUES (10, 6, 6, 1)
SET IDENTITY_INSERT [dbo].[Recipe] OFF
GO
SET IDENTITY_INSERT [dbo].[Restaurant] ON 

INSERT [dbo].[Restaurant] ([RestaurantID], [RestaurantName], [Address], [Phone], [Email]) VALUES (1, N'Nhà hàng Hoa Mai', N'123 Đường Láng, Hà Nội', N'02412345678', N'contact@hoamai.com')
INSERT [dbo].[Restaurant] ([RestaurantID], [RestaurantName], [Address], [Phone], [Email]) VALUES (2, N'Nhà hàng Sen Vàng', N'45 Nguyễn Huệ, TP.HCM', N'02898765432', N'info@senvang.com')
INSERT [dbo].[Restaurant] ([RestaurantID], [RestaurantName], [Address], [Phone], [Email]) VALUES (3, N'Nhà hàng Hoa Mai', N'123 Đường Láng, Hà Nội', N'02412345678', N'contact@hoamai.com')
INSERT [dbo].[Restaurant] ([RestaurantID], [RestaurantName], [Address], [Phone], [Email]) VALUES (4, N'Nhà hàng Sen Vàng', N'45 Nguyễn Huệ, TP.HCM', N'02898765432', N'info@senvang.com')
SET IDENTITY_INSERT [dbo].[Restaurant] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (1, N'Quản trị viên', N'admin@restaurant.com', N'0988888888', N'123', N'Quản lý', CAST(N'2026-03-31T17:13:36.860' AS DateTime), 1, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (2, N'Nguyễn Văn A', N'staff1@restaurant.com', N'0912345678', N'123', N'Nhân viên', CAST(N'2026-03-31T17:13:36.860' AS DateTime), 1, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (3, N'Trần Thị B', N'staff2@restaurant.com', N'0987654321', N'123', N'Nhân viên', CAST(N'2026-03-31T17:13:36.860' AS DateTime), 1, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (8, N'Viet', N'viet@restaurant.com', N'0900000000', N'tfv7LKe5OqSCqJ4eIoVSKdzHzQf8PO1dj7Vh8lhKAcA=', N'Nhân viên', CAST(N'2026-04-01T04:15:25.263' AS DateTime), 1, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (14, N'', N'', N'', N'', N'Nhân viên', CAST(N'2026-04-01T09:06:35.953' AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (19, N'Nguyễn Hoàng Việt', N'v@restaurant.com', N'09999', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Staff', CAST(N'2026-04-08T02:58:39.140' AS DateTime), 1, CAST(N'2026-04-08T03:42:11.840' AS DateTime))
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Phone], [PasswordHash], [Role], [CreatedAt], [IsActive], [LastLogin]) VALUES (20, N'đạt', N'dat@restaurant.com', N'83438738348', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'User', CAST(N'2026-04-12T22:28:57.303' AS DateTime), 1, CAST(N'2026-04-14T15:28:35.757' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_MenuItem_CategoryID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_MenuItem_CategoryID] ON [dbo].[MenuItem]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MenuItem_RestaurantID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_MenuItem_RestaurantID] ON [dbo].[MenuItem]
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetail_MenuItemID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetail_MenuItemID] ON [dbo].[OrderDetail]
(
	[MenuItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetail_OrderID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetail_OrderID] ON [dbo].[OrderDetail]
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_CustomerID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerID] ON [dbo].[Orders]
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_OrderDate]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_OrderDate] ON [dbo].[Orders]
(
	[OrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_RestaurantID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_RestaurantID] ON [dbo].[Orders]
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_StaffID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_StaffID] ON [dbo].[Orders]
(
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Orders_Status]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_Status] ON [dbo].[Orders]
(
	[Status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Payment_TransactionID]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Payment_TransactionID] ON [dbo].[Payment]
(
	[TransactionID] ASC
)
WHERE ([TransactionID] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534C301FE7F]    Script Date: 4/14/2026 3:42:44 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Email]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Role]    Script Date: 4/14/2026 3:42:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_Role] ON [dbo].[Users]
(
	[Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Feedback] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Ingredient] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Ingredient] ADD  CONSTRAINT [DF_Ingredient_MinStock]  DEFAULT ((0)) FOR [MinStock]
GO
ALTER TABLE [dbo].[InventoryTransaction] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[MenuItem] ADD  DEFAULT ((1)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[MenuItem] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[OrderHistory] ADD  DEFAULT (getdate()) FOR [ChangedAt]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Payment] ADD  DEFAULT (getdate()) FOR [PaymentTime]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[DiningTable]  WITH CHECK ADD FOREIGN KEY([RestaurantID])
REFERENCES [dbo].[Restaurant] ([RestaurantID])
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[InventoryTransaction]  WITH CHECK ADD FOREIGN KEY([IngredientID])
REFERENCES [dbo].[Ingredient] ([IngredientID])
GO
ALTER TABLE [dbo].[InventoryTransaction]  WITH CHECK ADD FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[MenuItem]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[MenuItem]  WITH CHECK ADD FOREIGN KEY([RestaurantID])
REFERENCES [dbo].[Restaurant] ([RestaurantID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([MenuItemID])
REFERENCES [dbo].[MenuItem] ([MenuItemID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderHistory]  WITH CHECK ADD FOREIGN KEY([ChangedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[OrderHistory]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([RestaurantID])
REFERENCES [dbo].[Restaurant] ([RestaurantID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([TableID])
REFERENCES [dbo].[DiningTable] ([TableID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Promotion] FOREIGN KEY([PromotionID])
REFERENCES [dbo].[Promotion] ([PromotionID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Promotion]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[PromotionDetail]  WITH CHECK ADD FOREIGN KEY([MenuItemID])
REFERENCES [dbo].[MenuItem] ([MenuItemID])
GO
ALTER TABLE [dbo].[PromotionDetail]  WITH CHECK ADD FOREIGN KEY([PromotionID])
REFERENCES [dbo].[Promotion] ([PromotionID])
GO
ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD FOREIGN KEY([IngredientID])
REFERENCES [dbo].[Ingredient] ([IngredientID])
GO
ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD FOREIGN KEY([MenuItemID])
REFERENCES [dbo].[MenuItem] ([MenuItemID])
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD FOREIGN KEY([RestaurantID])
REFERENCES [dbo].[Restaurant] ([RestaurantID])
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
/****** Object:  StoredProcedure [dbo].[sp_Orders_GetAll]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Orders_GetAll
CREATE PROCEDURE [dbo].[sp_Orders_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.*, c.FullName AS CustomerName, r.RestaurantName, t.TableNumber, s.Position
    FROM [dbo].[Orders] o
    LEFT JOIN [dbo].[Users] c ON o.CustomerID = c.UserID
    LEFT JOIN [dbo].[Restaurant] r ON o.RestaurantID = r.RestaurantID
    LEFT JOIN [dbo].[DiningTable] t ON o.TableID = t.TableID
    LEFT JOIN [dbo].[Staff] s ON o.StaffID = s.StaffID
    ORDER BY o.OrderDate DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Orders_GetByTable]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Orders_GetByTable
CREATE PROCEDURE [dbo].[sp_Orders_GetByTable]
    @TableID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.*, c.FullName AS CustomerName, r.RestaurantName, t.TableNumber
    FROM [dbo].[Orders] o
    LEFT JOIN [dbo].[Users] c ON o.CustomerID = c.UserID
    LEFT JOIN [dbo].[Restaurant] r ON o.RestaurantID = r.RestaurantID
    LEFT JOIN [dbo].[DiningTable] t ON o.TableID = t.TableID
    WHERE o.TableID = @TableID AND o.Status IN ('Pending', 'Cooking', 'Ready')
    ORDER BY o.OrderDate DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Orders_Insert]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================
-- STORED PROCEDURES - ORDERS
-- ============================================

-- sp_Orders_Insert
CREATE PROCEDURE [dbo].[sp_Orders_Insert]
    @CustomerID INT = NULL,
    @RestaurantID INT,
    @TableID INT = NULL,
    @OrderType NVARCHAR(20) = 'DineIn',
    @StaffID INT = NULL,
    @PromotionID INT = NULL,
    @ID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO [dbo].[Orders]
        (CustomerID, RestaurantID, TableID, OrderDate, OrderType, Status, TotalAmount, UpdatedAt, StaffID, PromotionID)
        VALUES
        (@CustomerID, @RestaurantID, @TableID, GETDATE(), @OrderType, 'Pending', 0, GETDATE(), @StaffID, @PromotionID);
        
        SET @ID = SCOPE_IDENTITY();
        
        -- Cập nhật trạng thái bàn
        IF @TableID IS NOT NULL
            UPDATE [dbo].[DiningTable] SET Status = 'Occupied' WHERE TableID = @TableID;
    END TRY
    BEGIN CATCH
        RAISERROR('Lỗi tạo order!', 16, 1);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Orders_UpdateStatus]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Orders_UpdateStatus
CREATE PROCEDURE [dbo].[sp_Orders_UpdateStatus]
    @OrderID INT,
    @NewStatus NVARCHAR(20),
    @ChangedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @OldStatus NVARCHAR(20);
        SELECT @OldStatus = Status FROM [dbo].[Orders] WHERE OrderID = @OrderID;
        
        UPDATE [dbo].[Orders]
        SET Status = @NewStatus, UpdatedAt = GETDATE()
        WHERE OrderID = @OrderID;
        
        -- Tạo OrderHistory
        INSERT INTO [dbo].[OrderHistory]
        (OrderID, OldStatus, NewStatus, ChangedAt, ChangedBy)
        VALUES
        (@OrderID, @OldStatus, @NewStatus, GETDATE(), @ChangedBy);
        
        -- Nếu Order Completed, cập nhật bàn thành Trống
        IF @NewStatus = 'Completed'
        BEGIN
            UPDATE [dbo].[DiningTable] 
            SET Status = 'Available' 
            WHERE TableID = (SELECT TableID FROM [dbo].[Orders] WHERE OrderID = @OrderID);
        END
    END TRY
    BEGIN CATCH
        RAISERROR('Lỗi cập nhật trạng thái order!', 16, 1);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_Delete]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Users_Delete
CREATE PROCEDURE [dbo].[sp_Users_Delete]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM [dbo].[Users] WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        RAISERROR('Lỗi xóa user!', 16, 1);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_GetAll]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Users_GetAll
CREATE PROCEDURE [dbo].[sp_Users_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[Users] ORDER BY UserID DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_GetByEmail]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Users_GetByEmail
CREATE PROCEDURE [dbo].[sp_Users_GetByEmail]
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[Users] WHERE Email = @Email;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_GetByID]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Users_GetByID
CREATE PROCEDURE [dbo].[sp_Users_GetByID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[Users] WHERE UserID = @UserID;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_Insert]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================
-- STORED PROCEDURES - USER
-- ============================================

-- sp_Users_Insert
CREATE PROCEDURE [dbo].[sp_Users_Insert]
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(20) = 'User',
    @ID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Kiểm tra email đã tồn tại
        IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE Email = @Email)
        BEGIN
            RAISERROR('Email đã được sử dụng!', 16, 1);
            RETURN;
        END
        
        INSERT INTO [dbo].[Users] (FullName, Email, Phone, PasswordHash, Role, CreatedAt, IsActive)
        VALUES (@FullName, @Email, @Phone, @PasswordHash, @Role, GETDATE(), 1);
        
        SET @ID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        RAISERROR('Lỗi thêm user!', 16, 1);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_Login]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Users_Login
CREATE PROCEDURE [dbo].[sp_Users_Login]
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM [dbo].[Users] 
    WHERE Email = @Email 
    AND PasswordHash = @PasswordHash 
    AND IsActive = 1;
    
    -- Cập nhật LastLogin
    UPDATE [dbo].[Users] SET LastLogin = GETDATE() 
    WHERE Email = @Email AND PasswordHash = @PasswordHash;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Users_Update]    Script Date: 4/14/2026 3:42:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Users_Update
CREATE PROCEDURE [dbo].[sp_Users_Update]
    @UserID INT,
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Role NVARCHAR(20),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE [dbo].[Users]
        SET FullName = @FullName, Email = @Email, Phone = @Phone, 
            Role = @Role, IsActive = @IsActive
        WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        RAISERROR('Lỗi cập nhật user!', 16, 1);
    END CATCH
END;
GO
CREATE TRIGGER [dbo].[trg_Orders_After_Insert]
ON [dbo].[Orders]
AFTER INSERT
AS
BEGIN
    INSERT INTO [dbo].[OrderHistory] (OrderID, OldStatus, NewStatus, ChangedAt, ChangedBy)
    SELECT OrderID, NULL, Status, GETDATE(), StaffID FROM inserted;
END;
GO

-- trg_Orders_After_Update
CREATE TRIGGER [dbo].[trg_Orders_After_Update]
ON [dbo].[Orders]
AFTER UPDATE
AS
BEGIN
    -- Chỉ tạo history nếu status thay đổi
    IF UPDATE(Status)
    BEGIN
        INSERT INTO [dbo].[OrderHistory] (OrderID, OldStatus, NewStatus, ChangedAt, ChangedBy)
        SELECT i.OrderID, d.Status, i.Status, GETDATE(), i.StaffID
        FROM inserted i
        INNER JOIN deleted d ON i.OrderID = d.OrderID
        WHERE i.Status != d.Status;
    END
END;
GO

-- trg_Payment_After_Insert
CREATE TRIGGER [dbo].[trg_Payment_After_Insert]
ON [dbo].[Payment]
AFTER INSERT
AS
BEGIN
    -- Cập nhật trạng thái order thành Completed sau khi thanh toán
    UPDATE [dbo].[Orders]
    SET Status = 'Completed', UpdatedAt = GETDATE()
    WHERE OrderID IN (SELECT OrderID FROM inserted)
    AND Status != 'Completed';
END;
GO

-- trg_OrderDetail_After_Insert
CREATE TRIGGER [dbo].[trg_OrderDetail_After_Insert]
ON [dbo].[OrderDetail]
AFTER INSERT
AS
BEGIN
    -- Cập nhật TotalAmount của Order
    UPDATE [dbo].[Orders]
    SET TotalAmount = [dbo].[fn_GetTotalAmount](OrderID),
        UpdatedAt = GETDATE()
    WHERE OrderID IN (SELECT OrderID FROM inserted);
END;
GO
CREATE PROCEDURE [dbo].[sp_DiningTable_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        t.TableID, 
        t.RestaurantID, 
        t.TableNumber, 
        t.Capacity, 
        t.Status, 
        t.Notes,
        r.RestaurantName
    FROM [dbo].[DiningTable] t
    LEFT JOIN [dbo].[Restaurant] r ON t.RestaurantID = r.RestaurantID
    ORDER BY t.TableNumber ASC;
END;
GO

CREATE PROCEDURE [dbo].[sp_GetBillByTable]
    @TableID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        m.ItemName, 
        od.Quantity, 
        od.UnitPrice, 
        od.SubTotal 
    FROM Orders o
    INNER JOIN OrderDetail od ON o.OrderID = od.OrderID
    INNER JOIN MenuItem m ON od.MenuItemID = m.MenuItemID
    WHERE o.TableID = @TableID AND o.Status IN (N'Đang dùng', N'Chưa thanh toán')
END;
GO