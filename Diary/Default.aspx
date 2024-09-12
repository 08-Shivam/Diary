<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Diary.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
    <div class="row">
        <div class="row">
            <p class="h2 text-center" style="border-bottom: groove; padding: 5px; color: goldenrod"><span style="color: burlywood; font-family:'Calisto MT'">Digital</span> Diary</p>
        </div>
        <div class="col-sm-4"></div>
        <div class="col-sm-4 form-sec">
            
                <h3 class="text-center">Login here</h3>
                <hr />
                <asp:Label runat="server" ID="lblMessage" ForeColor="OrangeRed"></asp:Label>
                <div class="form-group">
                    <asp:Label runat="server" ID="Label2" AssociatedControlID="txtEmail" Text="User Id*"></asp:Label>
                    <asp:TextBox CssClass="form-control" runat="server" ID="txtEmail" placeholder="Enter Your User Id"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="Label3" AssociatedControlID="txtPassword" Text="User Password*"></asp:Label>
                    <asp:TextBox CssClass="form-control" runat="server" ID="txtPassword" TextMode="Password" placeholder="Enter Your Password"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                </div>
                <hr />
            
            <div class="row text-center" style="padding: 5px">
                <asp:HyperLink runat="server" NavigateUrl="Register.aspx">New User? Click to Register.</asp:HyperLink>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="col-sm-4">
            </div>
        </div>
    </div>
    <div class="row" style="height: 200px"></div>
</div>
</asp:Content>
