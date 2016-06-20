<%@ Page Title="Link" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication8._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        Eesnimi: <asp:TextBox ID="first" runat="server" />
                <asp:RequiredFieldValidator id="RequiredFieldValidatorFirst" runat="server"
                ControlToValidate="first"
                ErrorMessage="Sisesta eesnimi"
                ForeColor="Red">
                </asp:RequiredFieldValidator>
        <br />
        <br />
        Perenimi: <asp:TextBox ID="last" runat="server" />
                <asp:RequiredFieldValidator id="RequiredFieldValidatorLast" runat="server"
                ControlToValidate="last"
                ErrorMessage="Sisesta perenimi"
                ForeColor="Red">
                </asp:RequiredFieldValidator>
        <br />
        <br />
        Sünniaeg: <asp:TextBox ID="DOB" runat="server" type="date" value="1000-01-01"/>
   <asp:RangeValidator ID ="RequiredfieldValidatorDOB" runat ="server" 
       ControlToValidate="DOB" 
       ErrorMessage="Sisesta sünniaeg" 
       Type="Date" 
       MinimumValue="01/01/1900"
       MaximumValue="01/01/2100"
       ForeColor="Red"
 ></asp:RangeValidator>
        <br />
        <br />

        <div id ="div1" runat="server"></div>
           <asp:ValidationSummary 
                              id="ValidationSummary1"  
                              runat="server"
                              HeaderText="Nõutud väljad täitmata:"
                              ValidationGroup ="vgroup1"
                              ShowSummary="true" 
                              DisplayMode="BulletList"/>
        <div id ="div2" runat="server"></div>
           <asp:ValidationSummary 
                              id="ValidationSummary2"  
                              runat="server"
                              HeaderText="Nõutud väljad täitmata:"
                              ValidationGroup ="vgroup2"
                              ShowSummary="true"
               DisplayMode="BulletList" />
        <div id ="div3" runat="server"></div>
           <asp:ValidationSummary 
                              id="ValidationSummary3"  
                              runat="server"
                              HeaderText="Nõutud väljad täitmata:"
                              ValidationGroup ="vgroup3"
                              ShowSummary="true"
               DisplayMode="BulletList" />
        <div id ="div4" runat="server"></div>
           <asp:ValidationSummary 
                              id="ValidationSummary4"  
                              runat="server"
                              HeaderText="Nõutud väljad täitmata:"
                              ValidationGroup ="vgroup4"
                              ShowSummary="true"
               DisplayMode="BulletList" />
        <div id ="div5" runat="server"></div>
           <asp:ValidationSummary 
                              id="ValidationSummary5"  
                              runat="server"
                              HeaderText="Nõutud väljad täitmata:"
                              ValidationGroup ="vgroup5"
                              ShowSummary="true" 
               DisplayMode="BulletList"/>
        <div id ="div6" runat="server"></div>
        <div id ="div7" runat="server"></div>
        <div id ="div8" runat="server"></div>
        <div id ="div9" runat="server"></div>
        <div id ="div10" runat="server"></div>

        <asp:Button id="submit" Text="Sisesta" runat="server" OnClientClick=" return Validate()" onClick="submit_Click"/>
        <asp:Button ID="populate" Text="Populate" runat="server" OnClick ="populate_Click" causesValidation="false" />
    <asp:Label ID ="errorBox" runat="server" Visible="false" ForeColor="Red"></asp:Label>
      <script type="text/javascript">
        function Validate() {
            var isValid = false;
            isValid = Page_ClientValidate('vgroup1') && Page_ClientValidate('vgroup2') && Page_ClientValidate('vgroup3') && Page_ClientValidate('vgroup4') && Page_ClientValidate('vgroup5');
            /* (isValid) {
                isValid = Page_ClientValidate('vgroup2');
            }
            if (isValid) {
                isValid = Page_ClientValidate('vgroup3');
            }
            if (isValid) {
                isValid = Page_ClientValidate('vgroup4');
            }
            if (isValid) {
                isValid = Page_ClientValidate('vgroup5');
            }
            return isValid;*/
        }
        function Success() {
            alert("Andmed sisestatud. Teid suunatakse kohe tulemuste lehele");
        }
        
    </script>
</asp:Content>
