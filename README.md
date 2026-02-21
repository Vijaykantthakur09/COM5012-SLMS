# Smart Library Management System (SLMS)

## Module
COM5012 – Object Oriented Programming

## Overview
This project implements a partial Smart Library Management System (SLMS) using C#.  
The system demonstrates key Object-Oriented Programming (OOP) principles including:

- Inheritance
- Encapsulation
- Polymorphism

The system allows members to search, borrow, return, and reserve books while enforcing library rules.

---

## Features Implemented

- View all books with status (Available, Borrowed, Reserved)
- Search books by title or author
- Borrow books (maximum 5 per member)
- Return borrowed books
- Reserve books that are currently borrowed
- Automatic 3-day reservation expiry
- Overdue report generation
- Console-based interactive menu

---

## Object-Oriented Concepts Used

### Inheritance
`Member` class inherits from the base `User` class.

### Encapsulation
Private fields and controlled access via properties and methods.

### Polymorphism
Method overriding is used to define role behaviour dynamically.

---

## How to Run

From the project root:

```bash
dotnet run --project src/SLMS/SLMS.csproj