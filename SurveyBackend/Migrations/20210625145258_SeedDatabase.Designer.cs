﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Survey.Models;

namespace Survey.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20210625145258_SeedDatabase")]
    partial class SeedDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Survey.Models.DataGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.HasKey("Id");

                    b.ToTable("DataGroups");
                });

            modelBuilder.Entity("Survey.Models.Execution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasDatabaseName("UQ__Executio__C41E02890F7150F6");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("PossibleResponses");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponseString", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<int>("PossibleResponseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PossibleResponseId");

                    b.ToTable("PossibleResponseStrings");
                });

            modelBuilder.Entity("Survey.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Survey.Models.QuestionExecution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ExecutionId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionExecutions");
                });

            modelBuilder.Entity("Survey.Models.QuestionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.HasKey("Id");

                    b.ToTable("QuestionTypes");
                });

            modelBuilder.Entity("Survey.Models.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Count")
                        .HasColumnType("decimal(18,0)");

                    b.Property<int>("DataGroupId")
                        .HasColumnType("int");

                    b.Property<int>("PossibleResponseId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionExecutionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DataGroupId");

                    b.HasIndex("PossibleResponseId");

                    b.HasIndex("QuestionExecutionId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponse", b =>
                {
                    b.HasOne("Survey.Models.QuestionType", "QuestionType")
                        .WithMany("PossibleResponses")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_PossibleResponses_QuestionTypes")
                        .IsRequired();

                    b.Navigation("QuestionType");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponseString", b =>
                {
                    b.HasOne("Survey.Models.PossibleResponse", "PossibleResponse")
                        .WithMany("PossibleResponseStrings")
                        .HasForeignKey("PossibleResponseId")
                        .HasConstraintName("FK_PossibleResponseStrings_PossibleResponses")
                        .IsRequired();

                    b.Navigation("PossibleResponse");
                });

            modelBuilder.Entity("Survey.Models.Question", b =>
                {
                    b.HasOne("Survey.Models.QuestionType", "QuestionType")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_Questions_QuestionTypes")
                        .IsRequired();

                    b.Navigation("QuestionType");
                });

            modelBuilder.Entity("Survey.Models.QuestionExecution", b =>
                {
                    b.HasOne("Survey.Models.Execution", "Execution")
                        .WithMany("QuestionExecutions")
                        .HasForeignKey("ExecutionId")
                        .HasConstraintName("FK_QuestionExecutions_Executions")
                        .IsRequired();

                    b.HasOne("Survey.Models.Question", "Question")
                        .WithMany("QuestionExecutions")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_QuestionExecutions_Questions")
                        .IsRequired();

                    b.Navigation("Execution");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Survey.Models.Response", b =>
                {
                    b.HasOne("Survey.Models.DataGroup", "DataGroup")
                        .WithMany("Responses")
                        .HasForeignKey("DataGroupId")
                        .HasConstraintName("FK_Responses_DataGroups")
                        .IsRequired();

                    b.HasOne("Survey.Models.PossibleResponse", "PossibleResponse")
                        .WithMany("Responses")
                        .HasForeignKey("PossibleResponseId")
                        .HasConstraintName("FK_Responses_PossibleResponses")
                        .IsRequired();

                    b.HasOne("Survey.Models.QuestionExecution", "QuestionExecution")
                        .WithMany("Responses")
                        .HasForeignKey("QuestionExecutionId")
                        .HasConstraintName("FK_Responses_QuestionExecutions")
                        .IsRequired();

                    b.Navigation("DataGroup");

                    b.Navigation("PossibleResponse");

                    b.Navigation("QuestionExecution");
                });

            modelBuilder.Entity("Survey.Models.DataGroup", b =>
                {
                    b.Navigation("Responses");
                });

            modelBuilder.Entity("Survey.Models.Execution", b =>
                {
                    b.Navigation("QuestionExecutions");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponse", b =>
                {
                    b.Navigation("PossibleResponseStrings");

                    b.Navigation("Responses");
                });

            modelBuilder.Entity("Survey.Models.Question", b =>
                {
                    b.Navigation("QuestionExecutions");
                });

            modelBuilder.Entity("Survey.Models.QuestionExecution", b =>
                {
                    b.Navigation("Responses");
                });

            modelBuilder.Entity("Survey.Models.QuestionType", b =>
                {
                    b.Navigation("PossibleResponses");

                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
