using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class usertbcols1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "zipcode");

            migrationBuilder.AddColumn<string>(
                name: "aboutMe",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "alcoholConsumptionId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "annualIncomeId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "childrenId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "eduction",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "fetichesId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gelocationBydistance",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "genderId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "height",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "hideage",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "last_relationship",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "lookingGenderId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "myProfessionId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "physicalTypeId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profession",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "relationshipStatusId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "religionId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sexualOrientationId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "signId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "smokeId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "vaccineId",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "whereamiknow",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "userHobbies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hobbyId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userHobbies", x => x.id);
                    table.ForeignKey(
                        name: "FK_userHobbies_ddls_hobbyId",
                        column: x => x.hobbyId,
                        principalTable: "ddls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userHobbies_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userLookingRelations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    relationId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userLookingRelations", x => x.id);
                    table.ForeignKey(
                        name: "FK_userLookingRelations_ddls_relationId",
                        column: x => x.relationId,
                        principalTable: "ddls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userLookingRelations_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userPersonalities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    personalityId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userPersonalities", x => x.id);
                    table.ForeignKey(
                        name: "FK_userPersonalities_ddls_personalityId",
                        column: x => x.personalityId,
                        principalTable: "ddls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userPersonalities_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userQualities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    qualityId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userQualities", x => x.id);
                    table.ForeignKey(
                        name: "FK_userQualities_ddls_qualityId",
                        column: x => x.qualityId,
                        principalTable: "ddls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userQualities_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_users_alcoholConsumptionId",
                table: "users",
                column: "alcoholConsumptionId");

            migrationBuilder.CreateIndex(
                name: "IX_users_annualIncomeId",
                table: "users",
                column: "annualIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_users_childrenId",
                table: "users",
                column: "childrenId");

            migrationBuilder.CreateIndex(
                name: "IX_users_fetichesId",
                table: "users",
                column: "fetichesId");

            migrationBuilder.CreateIndex(
                name: "IX_users_lookingGenderId",
                table: "users",
                column: "lookingGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_users_myProfessionId",
                table: "users",
                column: "myProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_users_physicalTypeId",
                table: "users",
                column: "physicalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_users_relationshipStatusId",
                table: "users",
                column: "relationshipStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_users_religionId",
                table: "users",
                column: "religionId");

            migrationBuilder.CreateIndex(
                name: "IX_users_sexualOrientationId",
                table: "users",
                column: "sexualOrientationId");

            migrationBuilder.CreateIndex(
                name: "IX_users_signId",
                table: "users",
                column: "signId");

            migrationBuilder.CreateIndex(
                name: "IX_users_smokeId",
                table: "users",
                column: "smokeId");

            migrationBuilder.CreateIndex(
                name: "IX_users_vaccineId",
                table: "users",
                column: "vaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_userHobbies_hobbyId",
                table: "userHobbies",
                column: "hobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_userHobbies_userId",
                table: "userHobbies",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userLookingRelations_relationId",
                table: "userLookingRelations",
                column: "relationId");

            migrationBuilder.CreateIndex(
                name: "IX_userLookingRelations_userId",
                table: "userLookingRelations",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userPersonalities_personalityId",
                table: "userPersonalities",
                column: "personalityId");

            migrationBuilder.CreateIndex(
                name: "IX_userPersonalities_userId",
                table: "userPersonalities",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userQualities_qualityId",
                table: "userQualities",
                column: "qualityId");

            migrationBuilder.CreateIndex(
                name: "IX_userQualities_userId",
                table: "userQualities",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_alcoholConsumptionId",
                table: "users",
                column: "alcoholConsumptionId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_annualIncomeId",
                table: "users",
                column: "annualIncomeId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_childrenId",
                table: "users",
                column: "childrenId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_fetichesId",
                table: "users",
                column: "fetichesId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_lookingGenderId",
                table: "users",
                column: "lookingGenderId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_myProfessionId",
                table: "users",
                column: "myProfessionId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_physicalTypeId",
                table: "users",
                column: "physicalTypeId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_relationshipStatusId",
                table: "users",
                column: "relationshipStatusId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_religionId",
                table: "users",
                column: "religionId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_sexualOrientationId",
                table: "users",
                column: "sexualOrientationId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_signId",
                table: "users",
                column: "signId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_smokeId",
                table: "users",
                column: "smokeId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_vaccineId",
                table: "users",
                column: "vaccineId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_alcoholConsumptionId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_annualIncomeId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_childrenId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_fetichesId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_lookingGenderId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_myProfessionId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_physicalTypeId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_relationshipStatusId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_religionId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_sexualOrientationId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_signId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_smokeId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_vaccineId",
                table: "users");

            migrationBuilder.DropTable(
                name: "userHobbies");

            migrationBuilder.DropTable(
                name: "userLookingRelations");

            migrationBuilder.DropTable(
                name: "userPersonalities");

            migrationBuilder.DropTable(
                name: "userQualities");

            migrationBuilder.DropIndex(
                name: "IX_users_alcoholConsumptionId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_annualIncomeId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_childrenId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_fetichesId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_lookingGenderId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_myProfessionId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_physicalTypeId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_relationshipStatusId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_religionId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_sexualOrientationId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_signId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_smokeId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_vaccineId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "aboutMe",
                table: "users");

            migrationBuilder.DropColumn(
                name: "age",
                table: "users");

            migrationBuilder.DropColumn(
                name: "alcoholConsumptionId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "annualIncomeId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "childrenId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "eduction",
                table: "users");

            migrationBuilder.DropColumn(
                name: "fetichesId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "gelocationBydistance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "genderId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "height",
                table: "users");

            migrationBuilder.DropColumn(
                name: "hideage",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_relationship",
                table: "users");

            migrationBuilder.DropColumn(
                name: "lookingGenderId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "myProfessionId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "physicalTypeId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "profession",
                table: "users");

            migrationBuilder.DropColumn(
                name: "relationshipStatusId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "religionId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "sexualOrientationId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "signId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "smokeId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "username",
                table: "users");

            migrationBuilder.DropColumn(
                name: "vaccineId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "whereamiknow",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "zipcode",
                table: "users",
                newName: "email");
        }
    }
}
