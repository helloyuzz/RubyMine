using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Migrations {
    public class IssueMigration: Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            var sql = @"CREATE OR ALTER VIEW [dbo].[vwIssusTemp] AS SELECT cv.id,cv.customized_id as 'issue_id',cv.value as 'cf_22',k.tracker_id from custom_values cv inner join issues k on cv.customized_id=k.id where cv.value is not null and cv.value !='' and cv.customized_type='Issue' and cv.custom_field_id=22 order by cv.value";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"DROP VIEW vwIssusTemp");
        }
    }
}
