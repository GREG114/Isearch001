﻿// <auto-generated />
using System;
using Isearch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Isearch.Migrations
{
    [DbContext(typeof(IsearchContext))]
    [Migration("20200113020718_t6")]
    partial class t6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Isearch.Models.ITWork", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("Create_at");

                    b.Property<string>("Creator");

                    b.Property<DateTime>("Finish_at");

                    b.Property<string>("Status");

                    b.Property<string>("Target");

                    b.Property<string>("Title");

                    b.Property<DateTime>("Update_at");

                    b.Property<string>("Workclass");

                    b.Property<int>("satisfied");

                    b.HasKey("Id");

                    b.ToTable("ITWork");
                });

            modelBuilder.Entity("Isearch.Models.NTQ", b =>
                {
                    b.Property<string>("姓名")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SSID");

                    b.Property<DateTime>("updatetime");

                    b.Property<string>("总体满意度");

                    b.Property<string>("所在办公区域");

                    b.Property<string>("无线下载速度");

                    b.Property<string>("无线可访问性");

                    b.Property<string>("无线打开速度");

                    b.Property<string>("无线稳定性");

                    b.Property<bool>("是否使用有线");

                    b.Property<string>("有线下载速度");

                    b.Property<string>("有线可访问性");

                    b.Property<string>("有线打开速度");

                    b.Property<string>("有线稳定性");

                    b.Property<string>("部门");

                    b.HasKey("姓名");

                    b.ToTable("NTQ");
                });

            modelBuilder.Entity("Isearch.Models.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("培训时长");

                    b.Property<DateTime>("培训时间");

                    b.Property<string>("培训讲师");

                    b.Property<string>("整合信息");

                    b.Property<string>("课程名称");

                    b.HasKey("Id");

                    b.ToTable("Trainings");
                });

            modelBuilder.Entity("Isearch.Models.TrainingFeedBack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TrainingID");

                    b.Property<int>("fb1");

                    b.Property<int>("fb10");

                    b.Property<int>("fb11");

                    b.Property<int>("fb12");

                    b.Property<int>("fb13");

                    b.Property<int>("fb14");

                    b.Property<int>("fb15");

                    b.Property<string>("fb16");

                    b.Property<string>("fb17");

                    b.Property<string>("fb18");

                    b.Property<string>("fb19");

                    b.Property<int>("fb2");

                    b.Property<string>("fb20");

                    b.Property<string>("fb21");

                    b.Property<string>("fb22");

                    b.Property<string>("fb23");

                    b.Property<string>("fb24");

                    b.Property<string>("fb25");

                    b.Property<string>("fb26");

                    b.Property<string>("fb27");

                    b.Property<string>("fb28");

                    b.Property<string>("fb29");

                    b.Property<int>("fb3");

                    b.Property<string>("fb30");

                    b.Property<int>("fb4");

                    b.Property<int>("fb5");

                    b.Property<int>("fb6");

                    b.Property<int>("fb7");

                    b.Property<int>("fb8");

                    b.Property<int>("fb9");

                    b.Property<int>("真实培训时间");

                    b.HasKey("Id");

                    b.HasIndex("TrainingID");

                    b.ToTable("TrainingFeedBacks");
                });

            modelBuilder.Entity("Isearch.Models.TrainingFeedBack", b =>
                {
                    b.HasOne("Isearch.Models.Training", "Training")
                        .WithMany()
                        .HasForeignKey("TrainingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
