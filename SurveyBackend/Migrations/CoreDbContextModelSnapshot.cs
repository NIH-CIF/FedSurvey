﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Survey.Models;

namespace Survey.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    partial class CoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Survey.Models.DataGroups", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(300)")
                        .HasMaxLength(300)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("DataGroups");
                });

            modelBuilder.Entity("Survey.Models.Executions", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasName("UQ__Executio__C41E02890F7150F6");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponseStrings", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(300)")
                        .HasMaxLength(300)
                        .IsUnicode(false);

                    b.Property<int>("PossibleResponseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PossibleResponseId");

                    b.ToTable("PossibleResponseStrings");
                });

            modelBuilder.Entity("Survey.Models.PossibleResponses", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("PossibleResponses");
                });

            modelBuilder.Entity("Survey.Models.QuestionExecutions", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

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

            modelBuilder.Entity("Survey.Models.QuestionTypes", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(300)")
                        .HasMaxLength(300)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("QuestionTypes");
                });

            modelBuilder.Entity("Survey.Models.Questions", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Survey.Models.Responses", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Count")
                        .HasColumnType("decimal(18, 0)");

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

            modelBuilder.Entity("Survey.Models.PossibleResponseStrings", b =>
                {
                    b.HasOne("Survey.Models.PossibleResponses", "PossibleResponse")
                        .WithMany("PossibleResponseStrings")
                        .HasForeignKey("PossibleResponseId")
                        .HasConstraintName("FK_PossibleResponseStrings_PossibleResponses")
                        .IsRequired();
                });

            modelBuilder.Entity("Survey.Models.PossibleResponses", b =>
                {
                    b.HasOne("Survey.Models.QuestionTypes", "QuestionType")
                        .WithMany("PossibleResponses")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_PossibleResponses_QuestionTypes")
                        .IsRequired();
                });

            modelBuilder.Entity("Survey.Models.QuestionExecutions", b =>
                {
                    b.HasOne("Survey.Models.Executions", "Execution")
                        .WithMany("QuestionExecutions")
                        .HasForeignKey("ExecutionId")
                        .HasConstraintName("FK_QuestionExecutions_Executions")
                        .IsRequired();

                    b.HasOne("Survey.Models.Questions", "Question")
                        .WithMany("QuestionExecutions")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_QuestionExecutions_Questions")
                        .IsRequired();
                });

            modelBuilder.Entity("Survey.Models.Questions", b =>
                {
                    b.HasOne("Survey.Models.QuestionTypes", "QuestionType")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionTypeId")
                        .HasConstraintName("FK_Questions_QuestionTypes")
                        .IsRequired();
                });

            modelBuilder.Entity("Survey.Models.Responses", b =>
                {
                    b.HasOne("Survey.Models.DataGroups", "DataGroup")
                        .WithMany("Responses")
                        .HasForeignKey("DataGroupId")
                        .HasConstraintName("FK_Responses_DataGroups")
                        .IsRequired();

                    b.HasOne("Survey.Models.PossibleResponses", "PossibleResponse")
                        .WithMany("Responses")
                        .HasForeignKey("PossibleResponseId")
                        .HasConstraintName("FK_Responses_PossibleResponses")
                        .IsRequired();

                    b.HasOne("Survey.Models.QuestionExecutions", "QuestionExecution")
                        .WithMany("Responses")
                        .HasForeignKey("QuestionExecutionId")
                        .HasConstraintName("FK_Responses_QuestionExecutions")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
