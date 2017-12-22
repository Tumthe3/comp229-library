﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class add_book : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        string theme;
        theme = (string)Session["theme"];

        if (!string.IsNullOrEmpty(theme))
        {
            Page.Theme = theme;
        }
        else
        {
            Page.Theme = "light";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGenres();
        }
    }

    private void BindGenres()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Library"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionString);

        SqlCommand comm = new SqlCommand("select Id, Genre from Genres", conn);

        try
        {
            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            ddlGenre.DataSource = reader;
            ddlGenre.DataValueField = "Id";
            ddlGenre.DataTextField = "Genre";
            ddlGenre.DataBind();
            reader.Close();
        }
        finally
        {
            conn.Close();
        }
    }

    protected void CheckFriendName(object sender, ServerValidateEventArgs e)
    {
        if (radLendedYes.Checked && string.IsNullOrEmpty(txtFriendName.Text))
        {
            e.IsValid = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string title, author, isbn, genre, friend, comments;
            int numPages;
            Book newBook;

            title = bbiEntry.Title.Text;
            author = bbiEntry.Author.Text;
            isbn = bbiEntry.Isbn.Text;
            genre = ddlGenre.SelectedValue;
            friend = txtFriendName.Text;
            comments = txtComments.Text;

            if (!Int32.TryParse(txtPages.Text, out numPages))
            {
                numPages = 1;
            }

            newBook = new Book(title, author, genre, numPages, isbn, friend, comments);
        }
    }
}