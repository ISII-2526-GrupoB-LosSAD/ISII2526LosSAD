INSERT INTO [dbo].[AspNetUsers] ([Id], [CustomerUserName], [CustomerUserSurname], [CustomerCountry], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1', N'Elena', N'Navarro Martínez', NULL, NULL, NULL, N'elena@uclm.es', NULL, 1, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 1)
INSERT INTO [dbo].[AspNetUsers] ([Id], [CustomerUserName], [CustomerUserSurname], [CustomerCountry], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2', N'Gregorio', N'Diaz Descalzo', NULL, NULL, NULL, N'gregorio@uclm.es', NULL, 1, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 2)
INSERT INTO [dbo].[AspNetUsers] ([Id], [CustomerUserName], [CustomerUserSurname], [CustomerCountry], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'3', N'Peter', N'Jackson', NULL, NULL, NULL, N'peter@uclm.es', NULL, 1, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 3)
INSERT INTO [dbo].[AspNetUsers] ([Id], [CustomerUserName], [CustomerUserSurname], [CustomerCountry], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'4', N'Daniel', N'Tierraseca Morcillo', NULL, NULL, NULL, N'daniel@uclm.es', NULL, 1, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 4)
INSERT INTO [dbo].[AspNetUsers] ([Id], [CustomerUserName], [CustomerUserSurname], [CustomerCountry], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'5', N'Antonio', N'Sanchez', NULL, NULL, NULL, N'antonio@uclm.es', NULL, 1, NULL, NULL, NULL, NULL, 1, 1, NULL, 1, 5)

SET IDENTITY_INSERT [dbo].[Scales] ON
INSERT INTO [dbo].[Scales] ([Id], [Name]) VALUES (1, N'Small Device')
INSERT INTO [dbo].[Scales] ([Id], [Name]) VALUES (2, N'Medium Device')
INSERT INTO [dbo].[Scales] ([Id], [Name]) VALUES (3, N'Large Device')
INSERT INTO [dbo].[Scales] ([Id], [Name]) VALUES (4, N'Gaming Console')
INSERT INTO [dbo].[Scales] ([Id], [Name]) VALUES (5, N'Laptop')
SET IDENTITY_INSERT [dbo].[Scales] OFF

SET IDENTITY_INSERT [dbo].[Repairs] ON
INSERT INTO [dbo].[Repairs] ([Id], [Cost], [Description], [Name], [ScaleId]) VALUES (1, 120.5, N'Replace cracked or broken screen', N'Screen Replacement', 1)
INSERT INTO [dbo].[Repairs] ([Id], [Cost], [Description], [Name], [ScaleId]) VALUES (2, 75, N'Install a new battery', N'Battery Replacement', 2)
INSERT INTO [dbo].[Repairs] ([Id], [Cost], [Description], [Name], [ScaleId]) VALUES (3, 250, N'Fix logic board issues', N'Motherboard Repair', 3)
INSERT INTO [dbo].[Repairs] ([Id], [Cost], [Description], [Name], [ScaleId]) VALUES (5, 95, N'Repair HDMI connection port', N'HDMI Port Repair', 4)
INSERT INTO [dbo].[Repairs] ([Id], [Cost], [Description], [Name], [ScaleId]) VALUES (11, 85, N'Replace faulty keyboard', N'Keyboard Replacement', 5)
SET IDENTITY_INSERT [dbo].[Repairs] OFF

SET IDENTITY_INSERT [dbo].[Receipts] ON
INSERT INTO [dbo].[Receipts] ([Id], [DeliveryAddress], [PaymentMethod], [ReceiptDate], [TotalPrice], [ApplicationUserId]) VALUES (2, N'123 Main St, New York', 2, N'2025-09-01 00:00:00', 195.5, N'1')
INSERT INTO [dbo].[Receipts] ([Id], [DeliveryAddress], [PaymentMethod], [ReceiptDate], [TotalPrice], [ApplicationUserId]) VALUES (3, N'Av. Insurgentes 55, CDMX', 3, N'2025-09-05 00:00:00', 250, N'2')
INSERT INTO [dbo].[Receipts] ([Id], [DeliveryAddress], [PaymentMethod], [ReceiptDate], [TotalPrice], [ApplicationUserId]) VALUES (4, N'Calle Gran Vía 14, Madrid', 1, N'2025-09-10 00:00:00', 175, N'3')
INSERT INTO [dbo].[Receipts] ([Id], [DeliveryAddress], [PaymentMethod], [ReceiptDate], [TotalPrice], [ApplicationUserId]) VALUES (5, N'12 King St, Toronto', 2, N'2025-09-15 00:00:00', 95, N'4')
INSERT INTO [dbo].[Receipts] ([Id], [DeliveryAddress], [PaymentMethod], [ReceiptDate], [TotalPrice], [ApplicationUserId]) VALUES (6, N'45 Baker St, London', 2, N'2025-09-20 00:00:00', 205, N'5')
SET IDENTITY_INSERT [dbo].[Receipts] OFF

INSERT INTO [dbo].[Receiptitems] ([ReceiptId], [RepairId], [Model]) VALUES (2, 1, N'iPhone 13')
INSERT INTO [dbo].[Receiptitems] ([ReceiptId], [RepairId], [Model]) VALUES (3, 2, N'iPad Air')
INSERT INTO [dbo].[Receiptitems] ([ReceiptId], [RepairId], [Model]) VALUES (4, 3, N'Samsung Galaxy S21')
INSERT INTO [dbo].[Receiptitems] ([ReceiptId], [RepairId], [Model]) VALUES (5, 5, N'PlayStation 5')
INSERT INTO [dbo].[Receiptitems] ([ReceiptId], [RepairId], [Model]) VALUES (6, 11, N'Dell XPS 13')

SET IDENTITY_INSERT [dbo].[Models] ON
INSERT INTO [dbo].[Models] ([Id], [NameModel]) VALUES (1, N'Galaxy S Series')
INSERT INTO [dbo].[Models] ([Id], [NameModel]) VALUES (2, N'iPhone 15')
INSERT INTO [dbo].[Models] ([Id], [NameModel]) VALUES (3, N'Pixel 8')
INSERT INTO [dbo].[Models] ([Id], [NameModel]) VALUES (4, N'ThinkPad X1')
INSERT INTO [dbo].[Models] ([Id], [NameModel]) VALUES (5, N'MacBook Pro M3')
SET IDENTITY_INSERT [dbo].[Models] OFF

SET IDENTITY_INSERT [dbo].[Devices] ON
INSERT INTO [dbo].[Devices] ([Id], [Brand], [Color], [Description], [Name], [priceForPurchase], [priceForRent], [Quality], [quauntityForPurchase], [quauntityForRent], [Year], [ModelId]) VALUES (1, N'Samsung', N'Black', N'High-end Android smartphone', N'Galaxy S24 Ultra', 1199.99, 89.99, N'A+', 50, 15, 2024, 1)
INSERT INTO [dbo].[Devices] ([Id], [Brand], [Color], [Description], [Name], [priceForPurchase], [priceForRent], [Quality], [quauntityForPurchase], [quauntityForRent], [Year], [ModelId]) VALUES (2, N'Apple', N'Silver', N'Flagship iOS smartphone', N'iPhone 15 Pro Max', 1399, 99.99, N'A+', 40, 10, 2024, 2)
INSERT INTO [dbo].[Devices] ([Id], [Brand], [Color], [Description], [Name], [priceForPurchase], [priceForRent], [Quality], [quauntityForPurchase], [quauntityForRent], [Year], [ModelId]) VALUES (4, N'Google', N'Blue', N'Google’s latest smartphone', N'Pixel 8 Pro', 999, 79.99, N'A', 60, 20, 2024, 3)
INSERT INTO [dbo].[Devices] ([Id], [Brand], [Color], [Description], [Name], [priceForPurchase], [priceForRent], [Quality], [quauntityForPurchase], [quauntityForRent], [Year], [ModelId]) VALUES (6, N'Lenovo', N'Black', N'Lightweight business laptop', N'ThinkPad X1 Gen11', 1899, 129.99, N'A+', 30, 5, 2023, 4)
INSERT INTO [dbo].[Devices] ([Id], [Brand], [Color], [Description], [Name], [priceForPurchase], [priceForRent], [Quality], [quauntityForPurchase], [quauntityForRent], [Year], [ModelId]) VALUES (7, N'Apple', N'Space Gray', N'Apple laptop with M3 chip', N'MacBook Pro 16"', 2499, 159.99, N'A+', 25, 4, 2024, 5)
SET IDENTITY_INSERT [dbo].[Devices] OFF

SET IDENTITY_INSERT [dbo].[Reviews] ON
INSERT INTO [dbo].[Reviews] ([ReviewId], [CustomerId], [DateOfReview], [OverallRating], [ReviewTitle], [ApplicationUserId]) VALUES (1, 1, N'2025-01-12 10:15:00', 5, N'Excellent Performance', N'1')
INSERT INTO [dbo].[Reviews] ([ReviewId], [CustomerId], [DateOfReview], [OverallRating], [ReviewTitle], [ApplicationUserId]) VALUES (2, 2, N'2025-02-05 09:40:00', 4, N'Very Satisfied', N'2')
INSERT INTO [dbo].[Reviews] ([ReviewId], [CustomerId], [DateOfReview], [OverallRating], [ReviewTitle], [ApplicationUserId]) VALUES (3, 3, N'2025-03-02 14:25:00', 4, N'Good Value for Money', N'3')
INSERT INTO [dbo].[Reviews] ([ReviewId], [CustomerId], [DateOfReview], [OverallRating], [ReviewTitle], [ApplicationUserId]) VALUES (4, 4, N'2025-04-10 18:00:00', 5, N'Perfect for Work', N'4')
INSERT INTO [dbo].[Reviews] ([ReviewId], [CustomerId], [DateOfReview], [OverallRating], [ReviewTitle], [ApplicationUserId]) VALUES (5, 5, N'2025-05-20 12:45:00', 3, N'Not bad but can improve', N'5')
SET IDENTITY_INSERT [dbo].[Reviews] OFF

INSERT INTO [dbo].[ReviewItems] ([DeviceId], [ReviewId], [Comments], [Rating]) VALUES (1, 1, N'Battery life is amazing.', 5)
INSERT INTO [dbo].[ReviewItems] ([DeviceId], [ReviewId], [Comments], [Rating]) VALUES (2, 2, N'Smooth performance and great screen', 5)
INSERT INTO [dbo].[ReviewItems] ([DeviceId], [ReviewId], [Comments], [Rating]) VALUES (4, 3, N'Camera quality is excellent.', 4)
INSERT INTO [dbo].[ReviewItems] ([DeviceId], [ReviewId], [Comments], [Rating]) VALUES (6, 4, N'Works flawlessly for my job.', 5)
INSERT INTO [dbo].[ReviewItems] ([DeviceId], [ReviewId], [Comments], [Rating]) VALUES (7, 5, N'Great display and speed.', 4)

SET IDENTITY_INSERT [dbo].[Purchases] ON
INSERT INTO [dbo].[Purchases] ([Id], [DeliveryAddress], [TotalPrice], [TotalQuantity], [PaymentMethod], [ReceiptDate], [ApplicationUserId]) VALUES (1, N'123 Elm St, NY', 2398.99, 2, 2, N'2025-01-15 10:30:00', N'1')
INSERT INTO [dbo].[Purchases] ([Id], [DeliveryAddress], [TotalPrice], [TotalQuantity], [PaymentMethod], [ReceiptDate], [ApplicationUserId]) VALUES (2, N'42 Market Rd, London', 1399, 1, 1, N'2025-02-10 14:12:00', N'2')
INSERT INTO [dbo].[Purchases] ([Id], [DeliveryAddress], [TotalPrice], [TotalQuantity], [PaymentMethod], [ReceiptDate], [ApplicationUserId]) VALUES (3, N'99 Avenue Q, Toronto', 999, 1, 2, N'2025-03-05 09:45:00', N'3')
INSERT INTO [dbo].[Purchases] ([Id], [DeliveryAddress], [TotalPrice], [TotalQuantity], [PaymentMethod], [ReceiptDate], [ApplicationUserId]) VALUES (4, N'5 Silicon Dr, San Jose', 1899, 1, 2, N'2025-03-22 16:10:00', N'4')
INSERT INTO [dbo].[Purchases] ([Id], [DeliveryAddress], [TotalPrice], [TotalQuantity], [PaymentMethod], [ReceiptDate], [ApplicationUserId]) VALUES (5, N'88 Sunset Blvd, Los Angeles', 4298, 2, 2, N'2025-05-28 17:20:00', N'5')
SET IDENTITY_INSERT [dbo].[Purchases] OFF

INSERT INTO [dbo].[PurchaseItems] ([DeviceId], [PurchaseId], [Description], [Price], [Quantity]) VALUES (1, 1, N'Galaxy S24 Ultra (x1)', 1199.99, 1)
INSERT INTO [dbo].[PurchaseItems] ([DeviceId], [PurchaseId], [Description], [Price], [Quantity]) VALUES (2, 2, N'Pixel 8 Pro (x1)', 999, 1)
INSERT INTO [dbo].[PurchaseItems] ([DeviceId], [PurchaseId], [Description], [Price], [Quantity]) VALUES (4, 3, N'iPhone 15 Pro Max (x1)', 1399, 1)
INSERT INTO [dbo].[PurchaseItems] ([DeviceId], [PurchaseId], [Description], [Price], [Quantity]) VALUES (6, 4, N'Pixel 8 Pro (x1)', 999, 1)
INSERT INTO [dbo].[PurchaseItems] ([DeviceId], [PurchaseId], [Description], [Price], [Quantity]) VALUES (7, 5, N'ThinkPad X1 Gen11 (x1)', 1899, 1)