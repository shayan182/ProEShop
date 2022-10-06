using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProEShop.Entities;

namespace ProEShop.DataLayer.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // i had an error so i create this just for test 
        builder.HasOne(x => x.Seller)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SellerId);

        // اگه محصولی به یک دسته بندی مرتبط باشه نباید بگذاریم اون دسته بندی حذف بشه 
        //چون وقتی حذف بشه زیر دسته های اون نیز هم پاک میشه 
        //در نتیجه در سیستم احتمال داره با یک حذف خیلی از روابط پاک بشه 
        //در یک چرخه قرار میگیره پس میگیم وفتی پاک شدی روی کاری نکن
        //اجازه نمیدیم تا وفتی که به مقداری وصل هست پاک بشه 
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.MainCategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
