<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Diary.Register" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="row">
                <p class="h2 text-center" style="border-bottom: groove; padding: 5px; color: goldenrod"><span style="color: burlywood; font-family: 'Calisto MT'">Digital</span> Diary</p>
            </div>
            <div class="col-sm-4 "></div>
            <div class="col-sm-4">
                <div class="row">
                    <p style="font-size: large;">Register for your <span style="color: burlywood; font-family: 'Century Gothic';">Digital</span> <span style="color: goldenrod">Diary</span></p>
                </div>
                <div class="row">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtRole" Text="Role Type"></asp:Label>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtRole" placeholder="Enter your role (Admin or User)"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label2" AssociatedControlID="txtName" Text="User Name"></asp:Label>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtName" placeholder="Enter Full Name"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtEmail" Text="Email"></asp:Label>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtEmail" placeholder="Enter Email Id"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label4" AssociatedControlID="txtPassword" Text="Password"></asp:Label>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtPassword" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                    </div>
                     &nbsp;
                    <div class="form-group">
                        <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Register" OnClick="Register_Click" Font-Size="Medium" />
                    </div>

                    <div class="row" style="padding: 5px"></div>
                    <div class="text-center form-group">
                        <asp:HyperLink runat="server" style="text-decoration:none; list-style:none" NavigateUrl="Default.aspx" Text="Already a member? Login here." CssClass="form-control"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="height: 50px"></div>
    </div>
</asp:Content>
