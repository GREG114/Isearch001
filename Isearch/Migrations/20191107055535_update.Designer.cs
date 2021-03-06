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
    [Migration("20191107055535_update")]
    partial class update
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<string>("有线下载速度");

                    b.Property<string>("有线可访问性");

                    b.Property<string>("有线打开速度");

                    b.Property<string>("有线稳定性");

                    b.Property<string>("部门");

                    b.HasKey("姓名");

                    b.ToTable("NTQ");
                });
#pragma warning restore 612, 618
        }
    }
}
