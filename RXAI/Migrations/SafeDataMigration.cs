using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RXAI.Migrations
{
    /// <inheritdoc />
    public partial class SafeDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. أولاً نقوم بإنشاء جدول مؤقت لحفظ البيانات من IngredientDetails
            migrationBuilder.Sql(@"
                SELECT 
                    Strength, 
                    CAST(Strength AS NVARCHAR(4)) AS StrengthString, 
                    StrengthUnit,
                    DrugBankID
                INTO #TempIngredientDetails
                FROM IngredientDetails
            ");

            // 2. إضافة الأعمدة الجديدة إلى TradeNames دون جعلها إلزامية في البداية
            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "TradeNames",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrengthUnit",
                table: "TradeNames",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            // 3. إنشاء علاقة بين PrescriptionDetails و Prescriptions
            // أولاً نحفظ العلاقات الحالية في جدول مؤقت
            migrationBuilder.Sql(@"
                SELECT 
                    pd.PrescriptionDetailID,
                    p.PrescriptionID
                INTO #TempPrescriptionRelation
                FROM PrescriptionDetails pd
                JOIN Prescriptions p ON pd.PrescriptionID = p.PrescriptionID
            ");

            // 4. تغيير نوع Strength في IngredientDetails من decimal إلى string
            // أولاً نزيل المفاتيح الأجنبية المرتبطة
            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID",
                table: "TradeNames");

            // 5. نعدّل عمود Strength في IngredientDetails
            migrationBuilder.AlterColumn<string>(
                name: "Strength",
                table: "IngredientDetails",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            // 6. ننقل البيانات المحولة من الجدول المؤقت
            migrationBuilder.Sql(@"
                UPDATE id
                SET id.Strength = tmp.StrengthString
                FROM IngredientDetails id
                JOIN #TempIngredientDetails tmp ON id.DrugBankID = tmp.DrugBankID AND id.StrengthUnit = tmp.StrengthUnit
            ");

            // 7. إضافة عمود PrescriptionDetailID إلى Prescription
            migrationBuilder.AddColumn<int>(
                name: "PrescriptionDetailID",
                table: "Prescriptions",
                type: "int",
                nullable: true);  // نجعله قابل للكون null مؤقتًا

            // 8. نعدّل هيكل العلاقات
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionDetails_Prescriptions_PrescriptionID",
                table: "PrescriptionDetails");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionDetails_PrescriptionID",
                table: "PrescriptionDetails");

            // 9. ننقل البيانات للعلاقة الجديدة
            migrationBuilder.Sql(@"
                UPDATE p
                SET p.PrescriptionDetailID = tmp.PrescriptionDetailID
                FROM Prescriptions p
                JOIN #TempPrescriptionRelation tmp ON p.PrescriptionID = tmp.PrescriptionID
            ");

            // 10. إضافة القيم في TradeNames
            migrationBuilder.Sql(@"
                UPDATE tn
                SET 
                    tn.Strength = id.Strength,
                    tn.StrengthUnit = id.StrengthUnit
                FROM TradeNames tn
                JOIN ActiveIngredients ai ON tn.DrugBankID = ai.DrugBankID
                JOIN IngredientDetails id ON ai.DrugBankID = id.DrugBankID
            ");

            // 11. الآن جعل الأعمدة الجديدة إلزامية
            migrationBuilder.AlterColumn<string>(
                name: "Strength",
                table: "TradeNames",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StrengthUnit",
                table: "TradeNames",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrescriptionDetailID",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // 12. نضيف الإندكسات والمفاتيح الأجنبية الجديدة
            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "Strength", "StrengthUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PrescriptionDetailID",
                table: "Prescriptions",
                column: "PrescriptionDetailID");

            // 13. نضيف المفاتيح الأجنبية للعلاقات الجديدة
            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_PrescriptionDetails_PrescriptionDetailID",
                table: "Prescriptions",
                column: "PrescriptionDetailID",
                principalTable: "PrescriptionDetails",
                principalColumn: "PrescriptionDetailID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_IngredientDetails_Strength_StrengthUnit",
                table: "TradeNames",
                columns: new[] { "Strength", "StrengthUnit" },
                principalTable: "IngredientDetails",
                principalColumns: new[] { "Strength", "StrengthUnit" },
                onDelete: ReferentialAction.Cascade);

            // 14. حذف الجداول المؤقتة
            migrationBuilder.Sql("DROP TABLE #TempIngredientDetails");
            migrationBuilder.Sql("DROP TABLE #TempPrescriptionRelation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // نعيد البيانات والهيكل للوضع السابق
            // أولاً ننشئ جداول مؤقتة للبيانات
            migrationBuilder.Sql(@"
                SELECT 
                    tn.AtcCode,
                    pd.PrescriptionDetailID,
                    p.PrescriptionID
                INTO #TempRelationsRestore
                FROM TradeNames tn
                JOIN IngredientDetails id ON tn.Strength = id.Strength AND tn.StrengthUnit = id.StrengthUnit
                LEFT JOIN PrescriptionDetails pd ON pd.PrescriptionDetailID IN (
                    SELECT PrescriptionDetailID FROM Prescriptions WHERE PrescriptionDetailID IS NOT NULL
                )
                LEFT JOIN Prescriptions p ON p.PrescriptionDetailID = pd.PrescriptionDetailID
            ");

            // نزيل المفاتيح الأجنبية الجديدة
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_PrescriptionDetails_PrescriptionDetailID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeNames_IngredientDetails_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_TradeNames_Strength_StrengthUnit",
                table: "TradeNames");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_PrescriptionDetailID",
                table: "Prescriptions");

            // نحوّل Strength مرة أخرى إلى decimal
            migrationBuilder.AlterColumn<decimal>(
                name: "Strength",
                table: "IngredientDetails",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            // نعيد البيانات باستخدام التحويل
            migrationBuilder.Sql(@"
                UPDATE id
                SET id.Strength = CAST(Strength AS decimal(10,2))
                FROM IngredientDetails id
            ");

            // نضيف عمود PrescriptionID إلى PrescriptionDetails 
            migrationBuilder.AddColumn<int>(
                name: "PrescriptionID",
                table: "PrescriptionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // نحذف الأعمدة من TradeNames
            migrationBuilder.DropColumn(
                name: "Strength",
                table: "TradeNames");

            migrationBuilder.DropColumn(
                name: "StrengthUnit",
                table: "TradeNames");

            // نحذف عمود PrescriptionDetailID من Prescriptions
            migrationBuilder.DropColumn(
                name: "PrescriptionDetailID",
                table: "Prescriptions");

            // نستعيد العلاقات القديمة
            migrationBuilder.Sql(@"
                UPDATE pd
                SET pd.PrescriptionID = tmp.PrescriptionID
                FROM PrescriptionDetails pd
                JOIN #TempRelationsRestore tmp ON pd.PrescriptionDetailID = tmp.PrescriptionDetailID
            ");

            // نعيد إنشاء الإندكسات والمفاتيح الأجنبية القديمة
            migrationBuilder.CreateIndex(
                name: "IX_TradeNames_DrugBankID",
                table: "TradeNames",
                column: "DrugBankID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_PrescriptionID",
                table: "PrescriptionDetails",
                column: "PrescriptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionDetails_Prescriptions_PrescriptionID",
                table: "PrescriptionDetails",
                column: "PrescriptionID",
                principalTable: "Prescriptions",
                principalColumn: "PrescriptionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeNames_ActiveIngredients_DrugBankID",
                table: "TradeNames",
                column: "DrugBankID",
                principalTable: "ActiveIngredients",
                principalColumn: "DrugBankID",
                onDelete: ReferentialAction.Restrict);

            // نحذف الجدول المؤقت
            migrationBuilder.Sql("DROP TABLE #TempRelationsRestore");
        }
    }
}