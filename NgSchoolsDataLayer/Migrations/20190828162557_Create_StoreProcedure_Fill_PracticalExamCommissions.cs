using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Create_StoreProcedure_Fill_PracticalExamCommissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries",
                column: "StudentsInGroupsId");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE FillMissingPracticalExamCommissionData
                AS 
                BEGIN

	                SET NOCOUNT ON;

	                DECLARE @StudentGroupId int;
	                DECLARE @ExamCommissionId int;
	                DECLARE @PracticalExamCommissionId int;

	                -- First, Copy All the Exam Commissions to Practical Exam Commissions, where needed

	                BEGIN

		                DECLARE cursor_StudentGroupsWithoutPEC CURSOR
		                FOR 
		                SELECT [STU].[Id], [STU].[ExamCommissionId], [STU].[PracticalExamCommissionId] 
		                FROM [StudentGroups] AS STU
		                WHERE 1=1
		                AND [STU].[ExamCommissionId] IS NOT NULL 
		                AND [STU].[PracticalExamCommissionId] IS NULL
		                AND [STU].[Status] = 0;

		                OPEN cursor_StudentGroupsWithoutPEC;

		                FETCH NEXT FROM cursor_StudentGroupsWithoutPEC INTO @StudentGroupId, @ExamCommissionId, @PracticalExamCommissionId;

		                WHILE @@FETCH_STATUS = 0
		                BEGIN
			                DECLARE @Name nvarchar(max);

			                SELECT @Name = [EXA].[Name]
			                FROM [ExamCommissions] AS EXA
			                WHERE 1=1
			                AND [EXA].[Id] = @ExamCommissionId
			                AND [EXA].[Status] = 0;

			                INSERT INTO [ExamCommissions]
			                (
				                --Id - column value is auto-generated
				                [Name],
				                [DateCreated],
				                [DateModified],
				                [Status]
			                )
			                VALUES
			                (
				                -- Id - int
				                @Name, -- Name - nvarchar
				                GetDate(), -- DateCreated - datetime2
				                NULL, -- DateModified - datetime2
				                0 -- Status - int
			                )

			                UPDATE [StudentGroups]
			                SET
				                [StudentGroups].[PracticalExamCommissionId] = SCOPE_IDENTITY(), -- int
				                [StudentGroups].[DateModified] = GetDate()
			                WHERE [StudentGroups].[Id] = @StudentGroupId;

			                FETCH NEXT FROM cursor_StudentGroupsWithoutPEC INTO @StudentGroupId, @ExamCommissionId, @PracticalExamCommissionId;
		                END;

		                CLOSE cursor_StudentGroupsWithoutPEC;

		                DEALLOCATE cursor_StudentGroupsWithoutPEC;

	                END;

	                -- Done with Copying Exam Commissions

	                -- Start Copying members

	                BEGIN

		                DECLARE cursor_MissingMembersInExamCommissions CURSOR
		                FOR
		                SELECT [STU].[Id], [STU].[ExamCommissionId], [STU].[PracticalExamCommissionId]
		                FROM [StudentGroups] AS STU
		                WHERE 1=1
		                AND [STU].[ExamCommissionId] IS NOT NULL 
		                AND [STU].[PracticalExamCommissionId] IS NOT NULL
		                AND [STU].[Status] = 0
		                AND NOT EXISTS (
			                SELECT DISTINCT [USE].[ExamCommissionId]
			                FROM [UserExamCommission] AS [USE]
			                WHERE 1=1
			                AND [USE].[Status] = 0
			                AND [USE].[ExamCommissionId] = [STU].[PracticalExamCommissionId]
		                );

		                OPEN cursor_MissingMembersInExamCommissions;

		                FETCH NEXT FROM cursor_MissingMembersInExamCommissions INTO @StudentGroupId, @ExamCommissionId, @PracticalExamCommissionId;

		                WHILE @@FETCH_STATUS = 0
		                BEGIN

			                INSERT INTO [UserExamCommission]
			                (
			                    --Id - column value is auto-generated
			                    [UserId],
			                    [ExamCommissionId],
			                    [DateCreated],
			                    [DateModified],
			                    [Status],
			                    [CommissionRole]
			                )
			                SELECT [USE].[UserId], @PracticalExamCommissionId, [USE].[DateCreated], null, 0, [USE].[CommissionRole]
			                FROM [UserExamCommission] AS [USE]
			                WHERE 1=1
			                AND [USE].[ExamCommissionId] = @ExamCommissionId
			                AND [USE].[Status] = 0;			

			                FETCH NEXT FROM cursor_MissingMembersInExamCommissions INTO @StudentGroupId, @ExamCommissionId, @PracticalExamCommissionId;
		                END;

		                CLOSE cursor_MissingMembersInExamCommissions;

		                DEALLOCATE cursor_MissingMembersInExamCommissions;

	                END;

                END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries",
                column: "StudentsInGroupsId",
                unique: true);

            migrationBuilder.Sql(@"DROP PROCEDURE [FillMissingPracticalExamCommissionData]");
        }
    }
}
