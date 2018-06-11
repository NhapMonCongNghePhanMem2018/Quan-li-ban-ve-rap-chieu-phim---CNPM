﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.DAO;
using GUI.DTO;

namespace GUI.frmAdminUserControls.DataUserControl
{
    public partial class MovieUC : UserControl
    {
        BindingSource movieList = new BindingSource();

        public MovieUC()
        {
            InitializeComponent();
            LoadMovie();
        }

        void LoadMovie()
        {
            dtgvMovie.DataSource = movieList;
            LoadMovieList();
            AddMovieBinding();
        }
        void LoadMovieList()
        {
            movieList.DataSource = MovieDAO.GetMovie();
        }
        private void btnShowMovie_Click(object sender, EventArgs e)
        {
            LoadMovieList();
        }
        void AddMovieBinding()
        {
            txtMovieID.DataBindings.Add("Text", dtgvMovie.DataSource, "Mã phim");
            txtMovieName.DataBindings.Add("Text", dtgvMovie.DataSource, "Tên phim");
            txtMovieDesc.DataBindings.Add("Text", dtgvMovie.DataSource, "Mô tả");
            txtMovieLength.DataBindings.Add("Text", dtgvMovie.DataSource, "Thời lượng");
            dtmMovieStart.DataBindings.Add("Value", dtgvMovie.DataSource, "Ngày khởi chiếu");
            dtmMovieEnd.DataBindings.Add("Value", dtgvMovie.DataSource, "Ngày kết thúc");
            txtMovieProductor.DataBindings.Add("Text", dtgvMovie.DataSource, "Sản xuất");
            txtMovieDirector.DataBindings.Add("Text", dtgvMovie.DataSource, "Đạo diễn");
            txtMovieYear.DataBindings.Add("Text", dtgvMovie.DataSource, "Năm SX");
            LoadGenreIntoCheckedList(clbMovieGenre);
        }
        void LoadGenreIntoCheckedList(CheckedListBox checkedList)
        {
            List<Genre> genreList = GenreDAO.GetListGenre();
            checkedList.DataSource = genreList;
            checkedList.DisplayMember = "Name";
        }
        private void txtMovieID_TextChanged(object sender, EventArgs e)
        //Use to binding the CheckedListBox Genre of Movie and picture of Movie
        {
            picFilm.Image = null;
            for (int i = 0; i < clbMovieGenre.Items.Count; i++)
            {
                clbMovieGenre.SetItemChecked(i, false);
                //Uncheck all CheckBox first
            }

            List<Genre> listGenreOfMovie = MovieByGenreDAO.GetListGenreByMovieID(txtMovieID.Text);
            for (int i = 0; i < clbMovieGenre.Items.Count; i++)
            {
                Genre genre = (Genre)clbMovieGenre.Items[i];
                foreach (Genre item in listGenreOfMovie)
                {
                    if (genre.ID == item.ID)
                    {
                        clbMovieGenre.SetItemChecked(i, true);
                        break;
                    }
                }
            }

            Movie movie = MovieDAO.GetMovieByID(txtMovieID.Text);
            if (movie.Poster != null)
                picFilm.Image = MovieDAO.byteArrayToImage(movie.Poster);
        }

        void InsertMovie(string id, string name, string desc, float length, DateTime startDate, DateTime endDate, string productor, string director, int year, byte[] image)
        {
            if (MovieDAO.InsertMovie(id, name, desc, length, startDate, endDate, productor, director, year, image))
            {
                MessageBox.Show("Thêm phim thành công");
            }
            else
            {
                MessageBox.Show("Thêm phim thất bại");
            }
        }
        void InsertMovie_Genre(string movieID, CheckedListBox checkedListBox)
        //Insert into table 'PhanLoaiPhim'
        {
            List<Genre> checkedGenreList = new List<Genre>();
            foreach (Genre checkedItem in checkedListBox.CheckedItems)
            {
                checkedGenreList.Add(checkedItem);
            }
            MovieByGenreDAO.InsertMovie_Genre(movieID, checkedGenreList);
        }

        private void btnUpLoadPictureFilm_Click(object sender, EventArgs e)
        {
            try
            {
                string filePathImage = null;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Pictures files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.jpg; *.jpeg; *.jpe; *.jfif; *.png|All files (*.*)|*.*";
                openFile.FilterIndex = 1;
                openFile.RestoreDirectory = true;
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    filePathImage = openFile.FileName;
                    picFilm.Image = Image.FromFile(filePathImage.ToString());
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void btnAddMovie_Click(object sender, EventArgs e)
        {
            string movieID = txtMovieID.Text;
            string movieName = txtMovieName.Text;
            string movieDesc = txtMovieDesc.Text;
            float movieLength = float.Parse(txtMovieLength.Text);
            DateTime startDate = dtmMovieStart.Value;
            DateTime endDate = dtmMovieEnd.Value;
            string productor = txtMovieProductor.Text;
            string director = txtMovieDirector.Text;
            int year = int.Parse(txtMovieYear.Text);
            if (picFilm.Image == null) return;
            InsertMovie(movieID, movieName, movieDesc, movieLength, startDate, endDate, productor, director, year, MovieDAO.imageToByteArray(picFilm.Image));
            InsertMovie_Genre(movieID, clbMovieGenre);
            LoadMovieList();
        }

        void UpdateMovie(string id, string name, string desc, float length, DateTime startDate, DateTime endDate, string productor, string director, int year, byte[] image)
        {
            if (MovieDAO.UpdateMovie(id, name, desc, length, startDate, endDate, productor, director, year, image))
            {
                MessageBox.Show("Sửa phim thành công");
            }
            else
            {
                MessageBox.Show("Sửa phim thất bại");
            }
        }
        void UpdateMovie_Genre(string movieID, CheckedListBox checkedListBox)
        {
            List<Genre> checkedGenreList = new List<Genre>();
            foreach (Genre checkedItem in checkedListBox.CheckedItems)
            {
                checkedGenreList.Add(checkedItem);
            }
            MovieByGenreDAO.UpdateMovie_Genre(movieID, checkedGenreList);
        }
        private void btnUpdateMovie_Click(object sender, EventArgs e)
        {
            string movieID = txtMovieID.Text;
            string movieName = txtMovieName.Text;
            string movieDesc = txtMovieDesc.Text;
            float movieLength = float.Parse(txtMovieLength.Text);
            DateTime startDate = dtmMovieStart.Value;
            DateTime endDate = dtmMovieEnd.Value;
            string productor = txtMovieProductor.Text;
            string director = txtMovieDirector.Text;
            int year = int.Parse(txtMovieYear.Text);
            if (picFilm.Image == null) return;
            UpdateMovie(movieID, movieName, movieDesc, movieLength, startDate, endDate, productor, director, year, MovieDAO.imageToByteArray(picFilm.Image));
            UpdateMovie_Genre(movieID, clbMovieGenre);
            LoadMovieList();
        }

        void DeleteMovie(string id)
        {
            if (MovieDAO.DeleteMovie(id))
            {
                MessageBox.Show("Xóa phim thành công");
            }
            else
            {
                MessageBox.Show("Xóa phim thất bại");
            }
        }
        private void btnDeleteMovie_Click(object sender, EventArgs e)
        {
            string movieID = txtMovieID.Text;
            DeleteMovie(movieID);
            LoadMovieList();
        }
    }
}