using System.Collections.Generic;
using Repository.Interface; 
using Repository.Schema; 

namespace Repository.Implementation.PgSQL
{
   public class StaffMasterRepository : PgSQLContext, IStaffMasterRepository
   {
        public StaffMasterRepository(string connectionString)
          : base(connectionString)
        {

        }

        public StaffMasterModel Select(int id)
        {
         var sql = @"
SELECT * FROM test.staff_master 
WHERE Id = @id;";
           return Select<StaffMasterModel>(sql, new { id });
        }

        public List<StaffMasterModel> SelectList()
        {
         var sql = @"
SELECT * FROM test.staff_master 
ORDER BY id;";
           return SelectList<StaffMasterModel>(sql);
        }

        public int Insert(StaffMasterModel obj)
        {
         var sql = @"
INSERT INTO test.staff_master 
(first_name, surname, email, insert_date, salary)
VALUES
(@FirstName, @Surname, @Email, @InsertDate, @Salary);
RETURNING id();";
           return Insert<StaffMasterModel>(sql, obj);
        }

        public void InsertBulk(List<StaffMasterModel> listPoco)
        {
         var sql = @"
INSERT INTO test.staff_master 
(first_name, surname, email, insert_date, salary)
VALUES
(@FirstName, @Surname, @Email, @InsertDate, @Salary);";
           InsertBulk<StaffMasterModel>(sql, listPoco);
        }

        public void Update(StaffMasterModel obj)
        {
         var sql = @"
UPDATE test.staff_master 
SET 
first_name=@FirstName, surname=@Surname, email=@Email, insert_date=@InsertDate, salary=@Salary
WHERE id = @id";
           Update<StaffMasterModel>(sql, obj);
        }

        public void Delete(int id)
        {
         var sql = @"
DELETE FROM test.staff_master 
WHERE id = @id";
           Delete(sql, new { id });
        }
   }
}
