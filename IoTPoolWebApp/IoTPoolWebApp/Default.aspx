<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IoTPoolWebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1 class="text-center">IoT Pool Controls</h1>
        <p class="text-center">Pool Power:
            <asp:Label ID="lblPoolPower" runat="server" Text="No data"></asp:Label>
        </p>
        <p class="text-center">Pool Water Temp (C):
            <asp:Label ID="lblPoolWaterTemp" runat="server" Text="No data"></asp:Label>
        </p>
        <p class="text-center">Outside Air Temp (C):
            <asp:Label ID="lblOutsideAirTemp" runat="server" Text="No data"></asp:Label>
        </p>
        <p class="text-center">Last Updated (UTC):
            <asp:Label ID="lblLastUpdated" runat="server" Text="No data"></asp:Label>
        </p>
        <p class="text-center">
            <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh" />
        </p>
        <p class="text-center">
            <asp:Button ID="btnPoolOn" runat="server" OnClick="btnPoolOn_Click" Text="Pool On" />
        </p>
        <p class="text-center">
            <asp:Button ID="btnPoolOff" runat="server" Text="Pool Off" OnClick="btnPoolOff_Click" />
        </p>
    </div>

</asp:Content>
