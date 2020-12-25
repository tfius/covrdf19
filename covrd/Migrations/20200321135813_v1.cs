using Microsoft.EntityFrameworkCore.Migrations;

namespace covrd.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bibref0",
                columns: table => new
                {
                    Bibref0Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<long>(nullable: true),
                    Venue = table.Column<string>(nullable: true),
                    Volume = table.Column<string>(nullable: true),
                    Issn = table.Column<string>(nullable: true),
                    Pages = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bibref0", x => x.Bibref0Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostCode = table.Column<string>(nullable: true),
                    Settlement = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "MetadataOverviews",
                columns: table => new
                {
                    MetadataOverviewId = table.Column<string>(nullable: false),
                    sha = table.Column<string>(nullable: true),
                    Source_x = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Doi = table.Column<string>(nullable: true),
                    Pmcid = table.Column<string>(nullable: true),
                    Pubmed_id = table.Column<string>(nullable: true),
                    License = table.Column<string>(nullable: true),
                    Abstract = table.Column<string>(nullable: true),
                    Publish_time = table.Column<string>(nullable: true),
                    Authors = table.Column<string>(nullable: true),
                    Journal = table.Column<string>(nullable: true),
                    Msapid = table.Column<string>(nullable: true),
                    Whocovid = table.Column<string>(nullable: true),
                    HasFullText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataOverviews", x => x.MetadataOverviewId);
                });

            migrationBuilder.CreateTable(
                name: "Metadatas",
                columns: table => new
                {
                    MetadataId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadatas", x => x.MetadataId);
                });

            migrationBuilder.CreateTable(
                name: "Ref0",
                columns: table => new
                {
                    Ref0Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ref0", x => x.Ref0Id);
                });

            migrationBuilder.CreateTable(
                name: "BibEntries",
                columns: table => new
                {
                    BibEntriesId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bibref0Id = table.Column<long>(nullable: true),
                    Bibref1Bibref0Id = table.Column<long>(nullable: true),
                    Bibref2Bibref0Id = table.Column<long>(nullable: true),
                    Bibref3Bibref0Id = table.Column<long>(nullable: true),
                    Bibref4Bibref0Id = table.Column<long>(nullable: true),
                    Bibref5Bibref0Id = table.Column<long>(nullable: true),
                    Bibref6Bibref0Id = table.Column<long>(nullable: true),
                    Bibref7Bibref0Id = table.Column<long>(nullable: true),
                    Bibref8Bibref0Id = table.Column<long>(nullable: true),
                    Bibref9Bibref0Id = table.Column<long>(nullable: true),
                    Bibref10Bibref0Id = table.Column<long>(nullable: true),
                    Bibref11Bibref0Id = table.Column<long>(nullable: true),
                    Bibref12Bibref0Id = table.Column<long>(nullable: true),
                    Bibref13Bibref0Id = table.Column<long>(nullable: true),
                    Bibref14Bibref0Id = table.Column<long>(nullable: true),
                    Bibref15Bibref0Id = table.Column<long>(nullable: true),
                    Bibref16Bibref0Id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibEntries", x => x.BibEntriesId);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref0Id",
                        column: x => x.Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref10Bibref0Id",
                        column: x => x.Bibref10Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref11Bibref0Id",
                        column: x => x.Bibref11Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref12Bibref0Id",
                        column: x => x.Bibref12Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref13Bibref0Id",
                        column: x => x.Bibref13Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref14Bibref0Id",
                        column: x => x.Bibref14Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref15Bibref0Id",
                        column: x => x.Bibref15Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref16Bibref0Id",
                        column: x => x.Bibref16Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref1Bibref0Id",
                        column: x => x.Bibref1Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref2Bibref0Id",
                        column: x => x.Bibref2Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref3Bibref0Id",
                        column: x => x.Bibref3Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref4Bibref0Id",
                        column: x => x.Bibref4Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref5Bibref0Id",
                        column: x => x.Bibref5Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref6Bibref0Id",
                        column: x => x.Bibref6Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref7Bibref0Id",
                        column: x => x.Bibref7Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref8Bibref0Id",
                        column: x => x.Bibref8Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BibEntries_Bibref0_Bibref9Bibref0Id",
                        column: x => x.Bibref9Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Affiliation",
                columns: table => new
                {
                    AffiliationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Laboratory = table.Column<string>(nullable: true),
                    Institution = table.Column<string>(nullable: true),
                    LocationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affiliation", x => x.AffiliationId);
                    table.ForeignKey(
                        name: "FK_Affiliation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefEntries",
                columns: table => new
                {
                    RefEntriesId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Figref0Ref0Id = table.Column<long>(nullable: true),
                    Figref1Ref0Id = table.Column<long>(nullable: true),
                    Figref2Ref0Id = table.Column<long>(nullable: true),
                    Figref3Ref0Id = table.Column<long>(nullable: true),
                    Figref4Ref0Id = table.Column<long>(nullable: true),
                    Figref5Ref0Id = table.Column<long>(nullable: true),
                    Figref6Ref0Id = table.Column<long>(nullable: true),
                    Figref7Ref0Id = table.Column<long>(nullable: true),
                    Figref8Ref0Id = table.Column<long>(nullable: true),
                    Figref9Ref0Id = table.Column<long>(nullable: true),
                    Figref10Ref0Id = table.Column<long>(nullable: true),
                    Tabref0Ref0Id = table.Column<long>(nullable: true),
                    Tabref1Ref0Id = table.Column<long>(nullable: true),
                    Tabref2Ref0Id = table.Column<long>(nullable: true),
                    Tabref3Ref0Id = table.Column<long>(nullable: true),
                    Tabref4Ref0Id = table.Column<long>(nullable: true),
                    Tabref5Ref0Id = table.Column<long>(nullable: true),
                    Tabref6Ref0Id = table.Column<long>(nullable: true),
                    Tabref7Ref0Id = table.Column<long>(nullable: true),
                    Tabref8Ref0Id = table.Column<long>(nullable: true),
                    Tabref9Ref0Id = table.Column<long>(nullable: true),
                    Tabref10Ref0Id = table.Column<long>(nullable: true),
                    Tabref11Ref0Id = table.Column<long>(nullable: true),
                    Tabref12Ref0Id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefEntries", x => x.RefEntriesId);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref0Ref0Id",
                        column: x => x.Figref0Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref10Ref0Id",
                        column: x => x.Figref10Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref1Ref0Id",
                        column: x => x.Figref1Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref2Ref0Id",
                        column: x => x.Figref2Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref3Ref0Id",
                        column: x => x.Figref3Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref4Ref0Id",
                        column: x => x.Figref4Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref5Ref0Id",
                        column: x => x.Figref5Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref6Ref0Id",
                        column: x => x.Figref6Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref7Ref0Id",
                        column: x => x.Figref7Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref8Ref0Id",
                        column: x => x.Figref8Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Figref9Ref0Id",
                        column: x => x.Figref9Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref0Ref0Id",
                        column: x => x.Tabref0Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref10Ref0Id",
                        column: x => x.Tabref10Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref11Ref0Id",
                        column: x => x.Tabref11Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref12Ref0Id",
                        column: x => x.Tabref12Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref1Ref0Id",
                        column: x => x.Tabref1Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref2Ref0Id",
                        column: x => x.Tabref2Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref3Ref0Id",
                        column: x => x.Tabref3Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref4Ref0Id",
                        column: x => x.Tabref4Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref5Ref0Id",
                        column: x => x.Tabref5Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref6Ref0Id",
                        column: x => x.Tabref6Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref7Ref0Id",
                        column: x => x.Tabref7Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref8Ref0Id",
                        column: x => x.Tabref8Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefEntries_Ref0_Tabref9Ref0Id",
                        column: x => x.Tabref9Ref0Id,
                        principalTable: "Ref0",
                        principalColumn: "Ref0Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetadataAuthor",
                columns: table => new
                {
                    MetadataAuthorId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First = table.Column<string>(nullable: true),
                    Last = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(nullable: true),
                    AffiliationId = table.Column<long>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Bibref0Id = table.Column<long>(nullable: true),
                    MetadataId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataAuthor", x => x.MetadataAuthorId);
                    table.ForeignKey(
                        name: "FK_MetadataAuthor_Affiliation_AffiliationId",
                        column: x => x.AffiliationId,
                        principalTable: "Affiliation",
                        principalColumn: "AffiliationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetadataAuthor_Bibref0_Bibref0Id",
                        column: x => x.Bibref0Id,
                        principalTable: "Bibref0",
                        principalColumn: "Bibref0Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetadataAuthor_Metadatas_MetadataId",
                        column: x => x.MetadataId,
                        principalTable: "Metadatas",
                        principalColumn: "MetadataId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Papers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaperId = table.Column<string>(nullable: true),
                    MetadataId = table.Column<long>(nullable: true),
                    BibEntriesId = table.Column<long>(nullable: true),
                    RefEntriesId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Papers_BibEntries_BibEntriesId",
                        column: x => x.BibEntriesId,
                        principalTable: "BibEntries",
                        principalColumn: "BibEntriesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Papers_Metadatas_MetadataId",
                        column: x => x.MetadataId,
                        principalTable: "Metadatas",
                        principalColumn: "MetadataId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Papers_RefEntries_RefEntriesId",
                        column: x => x.RefEntriesId,
                        principalTable: "RefEntries",
                        principalColumn: "RefEntriesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Abstract",
                columns: table => new
                {
                    AbstractId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "ntext", nullable: true),
                    Section = table.Column<string>(nullable: true),
                    PaperId = table.Column<long>(nullable: true),
                    PaperId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abstract", x => x.AbstractId);
                    table.ForeignKey(
                        name: "FK_Abstract_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Abstract_Papers_PaperId1",
                        column: x => x.PaperId1,
                        principalTable: "Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Notes = table.Column<string>(nullable: true),
                    PaperId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Papers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "Papers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Span",
                columns: table => new
                {
                    SpanId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<long>(nullable: false),
                    End = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    RefId = table.Column<string>(nullable: true),
                    AbstractId = table.Column<long>(nullable: true),
                    AbstractId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Span", x => x.SpanId);
                    table.ForeignKey(
                        name: "FK_Span_Abstract_AbstractId",
                        column: x => x.AbstractId,
                        principalTable: "Abstract",
                        principalColumn: "AbstractId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Span_Abstract_AbstractId1",
                        column: x => x.AbstractId1,
                        principalTable: "Abstract",
                        principalColumn: "AbstractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abstract_PaperId",
                table: "Abstract",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_Abstract_PaperId1",
                table: "Abstract",
                column: "PaperId1");

            migrationBuilder.CreateIndex(
                name: "IX_Affiliation_LocationId",
                table: "Affiliation",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref0Id",
                table: "BibEntries",
                column: "Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref10Bibref0Id",
                table: "BibEntries",
                column: "Bibref10Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref11Bibref0Id",
                table: "BibEntries",
                column: "Bibref11Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref12Bibref0Id",
                table: "BibEntries",
                column: "Bibref12Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref13Bibref0Id",
                table: "BibEntries",
                column: "Bibref13Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref14Bibref0Id",
                table: "BibEntries",
                column: "Bibref14Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref15Bibref0Id",
                table: "BibEntries",
                column: "Bibref15Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref16Bibref0Id",
                table: "BibEntries",
                column: "Bibref16Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref1Bibref0Id",
                table: "BibEntries",
                column: "Bibref1Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref2Bibref0Id",
                table: "BibEntries",
                column: "Bibref2Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref3Bibref0Id",
                table: "BibEntries",
                column: "Bibref3Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref4Bibref0Id",
                table: "BibEntries",
                column: "Bibref4Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref5Bibref0Id",
                table: "BibEntries",
                column: "Bibref5Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref6Bibref0Id",
                table: "BibEntries",
                column: "Bibref6Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref7Bibref0Id",
                table: "BibEntries",
                column: "Bibref7Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref8Bibref0Id",
                table: "BibEntries",
                column: "Bibref8Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BibEntries_Bibref9Bibref0Id",
                table: "BibEntries",
                column: "Bibref9Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PaperId",
                table: "Comments",
                column: "PaperId");

            migrationBuilder.CreateIndex(
                name: "IX_MetadataAuthor_AffiliationId",
                table: "MetadataAuthor",
                column: "AffiliationId");

            migrationBuilder.CreateIndex(
                name: "IX_MetadataAuthor_Bibref0Id",
                table: "MetadataAuthor",
                column: "Bibref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetadataAuthor_MetadataId",
                table: "MetadataAuthor",
                column: "MetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_BibEntriesId",
                table: "Papers",
                column: "BibEntriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_MetadataId",
                table: "Papers",
                column: "MetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_RefEntriesId",
                table: "Papers",
                column: "RefEntriesId");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref0Ref0Id",
                table: "RefEntries",
                column: "Figref0Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref10Ref0Id",
                table: "RefEntries",
                column: "Figref10Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref1Ref0Id",
                table: "RefEntries",
                column: "Figref1Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref2Ref0Id",
                table: "RefEntries",
                column: "Figref2Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref3Ref0Id",
                table: "RefEntries",
                column: "Figref3Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref4Ref0Id",
                table: "RefEntries",
                column: "Figref4Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref5Ref0Id",
                table: "RefEntries",
                column: "Figref5Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref6Ref0Id",
                table: "RefEntries",
                column: "Figref6Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref7Ref0Id",
                table: "RefEntries",
                column: "Figref7Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref8Ref0Id",
                table: "RefEntries",
                column: "Figref8Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Figref9Ref0Id",
                table: "RefEntries",
                column: "Figref9Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref0Ref0Id",
                table: "RefEntries",
                column: "Tabref0Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref10Ref0Id",
                table: "RefEntries",
                column: "Tabref10Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref11Ref0Id",
                table: "RefEntries",
                column: "Tabref11Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref12Ref0Id",
                table: "RefEntries",
                column: "Tabref12Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref1Ref0Id",
                table: "RefEntries",
                column: "Tabref1Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref2Ref0Id",
                table: "RefEntries",
                column: "Tabref2Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref3Ref0Id",
                table: "RefEntries",
                column: "Tabref3Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref4Ref0Id",
                table: "RefEntries",
                column: "Tabref4Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref5Ref0Id",
                table: "RefEntries",
                column: "Tabref5Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref6Ref0Id",
                table: "RefEntries",
                column: "Tabref6Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref7Ref0Id",
                table: "RefEntries",
                column: "Tabref7Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref8Ref0Id",
                table: "RefEntries",
                column: "Tabref8Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefEntries_Tabref9Ref0Id",
                table: "RefEntries",
                column: "Tabref9Ref0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Span_AbstractId",
                table: "Span",
                column: "AbstractId");

            migrationBuilder.CreateIndex(
                name: "IX_Span_AbstractId1",
                table: "Span",
                column: "AbstractId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "MetadataAuthor");

            migrationBuilder.DropTable(
                name: "MetadataOverviews");

            migrationBuilder.DropTable(
                name: "Span");

            migrationBuilder.DropTable(
                name: "Affiliation");

            migrationBuilder.DropTable(
                name: "Abstract");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Papers");

            migrationBuilder.DropTable(
                name: "BibEntries");

            migrationBuilder.DropTable(
                name: "Metadatas");

            migrationBuilder.DropTable(
                name: "RefEntries");

            migrationBuilder.DropTable(
                name: "Bibref0");

            migrationBuilder.DropTable(
                name: "Ref0");
        }
    }
}
