using ShopOnlineApp.Data.EF;
using System.Threading.Tasks;

namespace ShopOnlineApp.Initialization
{
    public class SearchProductUtils : IStage
    {
        private readonly AppDbContext _dbContext;
        public int Order => 3;

        public SearchProductUtils(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync()
        {
          await  Task.CompletedTask;
        } 
        //    await _dbContext.Database.ExecuteSqlCommandAsync(@"CREATE OR REPLACE FUNCTION remove_accent(input text)
        //      RETURNS text
        //      SET SCHEMA 'public'
        //      IMMUTABLE
        //      STRICT
        //      LANGUAGE SQL AS 
        //      $$
        //        SELECT translate(
        //        input,
        //        'áàảãạâấầẩẫậăắằẳẵặäåāąÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÄÅĀĄéèẻẽẹêếềểễệëēĕėęÉÈẺẼẸÊẾỀỂỄỆËĒĔĖĘiíìỉĩịîïĩīĭIÍÌỈĨỊÎÏĨĪĬóòỏõọơớờởỡợôốồổỗộöōŏőÓÒỎÕỌƠỚỜỞỠỢÔỐỒỔỖỘÖŌŎŐùúûüũūŭůủụưứừửữựÙÚÛÜŨŪŬŮỦỤƯỨỪỬỮỰyýỳỷỹỵYÝỲỶỸỴđĐ',
        //        'aaaaaaaaaaaaaaaaaaaaaAAAAAAAAAAAAAAAAAAAAAeeeeeeeeeeeeeeeeEEEEEEEEEEEEEEEEiiiiiiiiiiiIIIIIIIIIIIoooooooooooooooooooooOOOOOOOOOOOOOOOOOOOOOuuuuuuuuuuuuuuuuUUUUUUUUUUUUUUUUyyyyyyYYYYYYdD'
        //        );
        //      $$;");
        //}
    }
}
