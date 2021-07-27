﻿// <auto-generated />
using System;
using FedSurvey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FedSurvey.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    partial class CoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FedSurvey.Models.DataGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DataGroups");
                });

            modelBuilder.Entity("FedSurvey.Models.DataGroupLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.HasIndex("ParentId");

                    b.ToTable("DataGroupLink");
                });

            modelBuilder.Entity("FedSurvey.Models.DataGroupString", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DataGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<bool>("Preferred")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DataGroupId");

                    b.ToTable("DataGroupStrings");
                });

            modelBuilder.Entity("FedSurvey.Models.Execution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasDatabaseName("UQ__Executio__C41E02890F7150F6");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("FedSurvey.Models.PossibleResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PartOfPercentage")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("PossibleResponses");
                });

            modelBuilder.Entity("FedSurvey.Models.PossibleResponseString", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("PossibleResponseId")
                        .HasColumnType("int");

                    b.Property<bool>("Preferred")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PossibleResponseId");

                    b.ToTable("PossibleResponseStrings");
                });

            modelBuilder.Entity("FedSurvey.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionExecution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExecutionId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionExecutions");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("QuestionTypes");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionTypeString", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<bool>("Preferred")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("QuestionTypeStrings");
                });

            modelBuilder.Entity("FedSurvey.Models.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Count")
                        .HasColumnType("decimal(19,12)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DataGroupId")
                        .HasColumnType("int");

                    b.Property<int>("PossibleResponseId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionExecutionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DataGroupId");

                    b.HasIndex("PossibleResponseId");

                    b.HasIndex("QuestionExecutionId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("FedSurvey.Models.ResponseDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Count")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DataGroupId")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PossibleResponseId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionExecutionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b
                        .HasAnnotation("Relational:SqlQuery", "SELECT\r\n                  Responses.Id,\r\n                  Responses.Count,\r\n                  Responses.QuestionExecutionId,\r\n                  Responses.PossibleResponseId,\r\n                  Responses.DataGroupId,\r\n                  CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 OR PossibleResponses.PartOfPercentage = 0 THEN 0 ELSE Responses.Count / SUM(QuestionExecutionResponses.Count) * 100 END AS Percentage\r\n                FROM Responses\r\n                JOIN PossibleResponses\r\n                ON PossibleResponses.Id = Responses.PossibleResponseId\r\n                LEFT JOIN (\r\n                    SELECT Responses.*\r\n                    FROM Responses\r\n                    JOIN PossibleResponses\r\n                        ON PossibleResponses.Id = Responses.PossibleResponseId\r\n                    WHERE PossibleResponses.PartOfPercentage = 1\r\n                ) QuestionExecutionResponses\r\n                ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId\r\n                    AND QuestionExecutionResponses.DataGroupId = Responses.DataGroupId\r\n                GROUP BY\r\n                Responses.Id,\r\n                Responses.Count,\r\n                Responses.QuestionExecutionId,\r\n                Responses.PossibleResponseId,\r\n                Responses.DataGroupId,\r\n                PossibleResponses.PartOfPercentage");
                });

            modelBuilder.Entity("FedSurvey.Models.ResultDTO", b =>
                {
                    b.Property<decimal>("Count")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DataGroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExecutionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExecutionTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PossibleResponseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionNumber")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b
                        .HasAnnotation("Relational:SqlQuery", "SELECT\r\n                    Executions.\"Key\" AS ExecutionName,\r\n                    Executions.OccurredTime AS ExecutionTime,\r\n                    PossibleResponseStrings.\"Name\" AS PossibleResponseName,\r\n                    Responses.Count,\r\n                    CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 OR PossibleResponses.PartOfPercentage = 0 THEN NULL ELSE Responses.Count / SUM(QuestionExecutionResponses.Count) * 100 END AS Percentage,\r\n                    QuestionExecutions.Body AS QuestionText,\r\n                    DataGroupStrings.\"Name\" AS DataGroupName,\r\n                    QuestionExecutions.QuestionId AS QuestionId,\r\n                    QuestionExecutions.Position AS QuestionNumber\r\n                FROM (\r\n                    SELECT\r\n                        SUM(Responses.Count) AS \"Count\",\r\n                        Responses.QuestionExecutionId,\r\n                        Responses.PossibleResponseId,\r\n                        Responses.DataGroupId\r\n                    FROM Responses\r\n                    GROUP BY\r\n                    Responses.QuestionExecutionId,\r\n                    Responses.PossibleResponseId,\r\n                    Responses.DataGroupId\r\n                ) Responses\r\n                JOIN PossibleResponses\r\n                ON PossibleResponses.Id = Responses.PossibleResponseId\r\n                JOIN PossibleResponseStrings\r\n                ON PossibleResponseStrings.PossibleResponseId = PossibleResponses.Id\r\n                    AND PossibleResponseStrings.Preferred = 1\r\n                JOIN QuestionExecutions\r\n                ON QuestionExecutions.Id = Responses.QuestionExecutionId\r\n                JOIN Executions\r\n                ON Executions.Id = QuestionExecutions.ExecutionId\r\n                JOIN DataGroups\r\n                ON DataGroups.Id = Responses.DataGroupId\r\n                JOIN DataGroupStrings\r\n                ON DataGroupStrings.DataGroupId = DataGroups.Id\r\n                    AND DataGroupStrings.Preferred = 1\r\n                LEFT JOIN (\r\n                    SELECT Responses.*\r\n                    FROM Responses\r\n                    JOIN PossibleResponses\r\n                        ON PossibleResponses.Id = Responses.PossibleResponseId\r\n                    WHERE PossibleResponses.PartOfPercentage = 1\r\n                ) QuestionExecutionResponses\r\n                ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId\r\n                    AND QuestionExecutionResponses.DataGroupId = Responses.DataGroupId\r\n                GROUP BY\r\n                Executions.\"Key\",\r\n                Executions.OccurredTime,\r\n                PossibleResponseStrings.\"Name\",\r\n                Responses.Count,\r\n                PossibleResponses.PartOfPercentage,\r\n                QuestionExecutions.Body,\r\n                DataGroupStrings.\"Name\",\r\n                QuestionExecutions.QuestionId,\r\n                QuestionExecutions.Position,\r\n                PossibleResponses.Id\r\n                ORDER BY\r\n                PossibleResponses.PartOfPercentage DESC,\r\n                PossibleResponses.Id ASC");
                });

            modelBuilder.Entity("FedSurvey.Models.DataGroupLink", b =>
                {
                    b.HasOne("FedSurvey.Models.DataGroup", "Child")
                        .WithMany("ChildLinks")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FedSurvey.Models.DataGroup", "Parent")
                        .WithMany("ParentLinks")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Child");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("FedSurvey.Models.DataGroupString", b =>
                {
                    b.HasOne("FedSurvey.Models.DataGroup", "DataGroup")
                        .WithMany("DataGroupStrings")
                        .HasForeignKey("DataGroupId")
                        .HasConstraintName("FK_DataGroupStrings_DataGroups_DataGroupId")
                        .IsRequired();

                    b.Navigation("DataGroup");
                });

            modelBuilder.Entity("FedSurvey.Models.PossibleResponse", b =>
                {
                    b.HasOne("FedSurvey.Models.QuestionType", "QuestionType")
                        .WithMany("PossibleResponses")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_PossibleResponses_QuestionTypes")
                        .IsRequired();

                    b.Navigation("QuestionType");
                });

            modelBuilder.Entity("FedSurvey.Models.PossibleResponseString", b =>
                {
                    b.HasOne("FedSurvey.Models.PossibleResponse", "PossibleResponse")
                        .WithMany("PossibleResponseStrings")
                        .HasForeignKey("PossibleResponseId")
                        .HasConstraintName("FK_PossibleResponseStrings_PossibleResponses")
                        .IsRequired();

                    b.Navigation("PossibleResponse");
                });

            modelBuilder.Entity("FedSurvey.Models.Question", b =>
                {
                    b.HasOne("FedSurvey.Models.QuestionType", "QuestionType")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_Questions_QuestionTypes")
                        .IsRequired();

                    b.Navigation("QuestionType");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionExecution", b =>
                {
                    b.HasOne("FedSurvey.Models.Execution", "Execution")
                        .WithMany("QuestionExecutions")
                        .HasForeignKey("ExecutionId")
                        .HasConstraintName("FK_QuestionExecutions_Executions")
                        .IsRequired();

                    b.HasOne("FedSurvey.Models.Question", "Question")
                        .WithMany("QuestionExecutions")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_QuestionExecutions_Questions")
                        .IsRequired();

                    b.Navigation("Execution");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionTypeString", b =>
                {
                    b.HasOne("FedSurvey.Models.QuestionType", "QuestionType")
                        .WithMany("QuestionTypeStrings")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_QuestionTypeStrings_QuestionTypes_QuestionTypeId")
                        .IsRequired();

                    b.Navigation("QuestionType");
                });

            modelBuilder.Entity("FedSurvey.Models.Response", b =>
                {
                    b.HasOne("FedSurvey.Models.DataGroup", "DataGroup")
                        .WithMany("Responses")
                        .HasForeignKey("DataGroupId")
                        .HasConstraintName("FK_Responses_DataGroups")
                        .IsRequired();

                    b.HasOne("FedSurvey.Models.PossibleResponse", "PossibleResponse")
                        .WithMany("Responses")
                        .HasForeignKey("PossibleResponseId")
                        .HasConstraintName("FK_Responses_PossibleResponses")
                        .IsRequired();

                    b.HasOne("FedSurvey.Models.QuestionExecution", "QuestionExecution")
                        .WithMany("Responses")
                        .HasForeignKey("QuestionExecutionId")
                        .HasConstraintName("FK_Responses_QuestionExecutions")
                        .IsRequired();

                    b.Navigation("DataGroup");

                    b.Navigation("PossibleResponse");

                    b.Navigation("QuestionExecution");
                });

            modelBuilder.Entity("FedSurvey.Models.DataGroup", b =>
                {
                    b.Navigation("ChildLinks");

                    b.Navigation("DataGroupStrings");

                    b.Navigation("ParentLinks");

                    b.Navigation("Responses");
                });

            modelBuilder.Entity("FedSurvey.Models.Execution", b =>
                {
                    b.Navigation("QuestionExecutions");
                });

            modelBuilder.Entity("FedSurvey.Models.PossibleResponse", b =>
                {
                    b.Navigation("PossibleResponseStrings");

                    b.Navigation("Responses");
                });

            modelBuilder.Entity("FedSurvey.Models.Question", b =>
                {
                    b.Navigation("QuestionExecutions");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionExecution", b =>
                {
                    b.Navigation("Responses");
                });

            modelBuilder.Entity("FedSurvey.Models.QuestionType", b =>
                {
                    b.Navigation("PossibleResponses");

                    b.Navigation("Questions");

                    b.Navigation("QuestionTypeStrings");
                });
#pragma warning restore 612, 618
        }
    }
}
