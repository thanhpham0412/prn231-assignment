﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repository.Interface;
using DAO;

namespace Repository
{
    public class BookRepository : IBookRepository
    {
        void IBookRepository.Delete(Book book) => BookDAO.Instance.Delete(book);

        List<Book> IBookRepository.GetAll() => BookDAO.Instance.GetAll();

        Book IBookRepository.GetById(int id) => BookDAO.Instance.GetById(id);

        void IBookRepository.Insert(Book book) => BookDAO.Instance.Insert(book);

        void IBookRepository.Save() =>BookDAO.Instance.Save();

        void IBookRepository.Update(Book book) => BookDAO.Instance.Update(book);
    }
}
