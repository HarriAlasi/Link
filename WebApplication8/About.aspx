<%@ Page Title="Tulemused" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplication8.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2> Tulemused </h2>
    <asp:Table ID="resultsTable" runat="server"  BorderWidth="2" GridLines="Both" BorderStyle="Solid">
        <asp:TableHeaderRow runat="server">
            <asp:TableHeaderCell HorizontalAlign="Center" verticalalign="Middle">
                URL
            </asp:TableHeaderCell>
            <asp:TableHeaderCell verticalalign="Middle" HorizontalAlign="Center" >
                Pealkiri
            </asp:TableHeaderCell>
            <asp:TableHeaderCell verticalalign="Middle" HorizontalAlign="Center" >
                Kirjeldus
            </asp:TableHeaderCell>
            <asp:TableHeaderCell verticalalign="Middle" HorizontalAlign="Center" >
                Omanik
            </asp:TableHeaderCell>
            <asp:TableHeaderCell verticalalign="Middle" HorizontalAlign="Center" >
                Kategooria
            </asp:TableHeaderCell>
                        <asp:TableHeaderCell verticalalign="Middle" HorizontalAlign="Center" >
                Punktisumma
            </asp:TableHeaderCell>
        </asp:TableHeaderRow>
        </asp:Table>
</asp:Content>
