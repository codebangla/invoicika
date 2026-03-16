![Invoicika](https://im.ages.io/zxV6jintl3)

# **Invoicika**

Invoicika is an advanced invoice management system built with Angular 16, ASP.NET Core Web API 10.0, and PostgreSQL. This system provides full-fledged invoicing capabilities, including unlimited invoice creation, customer management, PDF generation, and more. The frontend is built with Angular and utilizes NG-Zorro UI components for a modern, responsive design.

## Features

- **Create Unlimited Invoices**: Generate invoices without any restrictions.
- **Email Invoice**: Send invoices directly to customers via email.
- **PDF Generation**: Export invoices as PDFs for easy sharing and storage.
- **Customer Management**: Add, edit, and manage customers with multiple shipping addresses.
- **Authentication & Roles**: Secure login and role-based access control.
- **Customer Product Management**: Add products linked to customers for quick access.
- **Sign Up & Profile Management**: Users can sign up and manage their profile.
- **Image Upload**: Upload and manage profile pictures and invoice logos.
- **VAT Management**: Handle VAT for customer invoices.
- **Database**: Built to work with PostgreSQL.

![Description](https://i.imgur.com/uDmUb5U.png)

## Technologies Used

- **Frontend**: Angular with NG-Zorro (Responsive UI components).
- **Backend**: ASP.NET Core Web API 10.0 (Robust and scalable API layer).
- **Database**: PostgreSQL (Code First Migration).

![Description](https://i.imgur.com/0dwmGY1.png)

## How to Install (without Docker)

### Prerequisites

- **Node.js** (For Angular)
- **PostgreSQL** (For database)
- **.NET 10.0 SDK** (For ASP.NET Core)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/codebangla/invoicika.git
   cd invoicika
   ```

2. **Frontend (Angular)**:

   ```bash
   cd frontend
   npm install
   ng serve
   ```

3. **Backend (ASP.NET Core)**:

   ```bash
   cd backend
   dotnet restore
   ```

4. **Database Setup (PostgreSQL)**:

   - Ensure PostgreSQL is installed and running.
   - Update the connection strings in `appsettings.json` to configure your PostgreSQL database.

     Example:

     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Your PostgreSQL Server connection string"
     }
     ```

5. **Apply Migrations**:
   ```bash
   dotnet ef database update
   ```

### Running the Application

1. Run the **Angular frontend**:

   ```bash
   ng serve
   ```

2. Run the **ASP.NET Core backend**:

   ```bash
   dotnet run
   ```

   3. Open your browser and navigate to `http://localhost:4200` for the frontend. Login with `username: admin1, password: admin1` as admin or `username: employee1, password: employee1` as employee. The backend is at `http://localhost:5000/swagger/index.html`

3. To use SMTP with Outlook, you need to generate an app password if two-factor authentication (2FA) is enabled on your Microsoft account. Follow this [guide on how to get an app password](https://support.microsoft.com/en-us/account-billing/how-to-get-and-use-app-passwords-5896ed9b-4263-e681-128a-a6f2979a7944) for detailed steps.

   Make sure to replace the `"Password"` field in your `appsettings.js` with your generated app password.
   In your `appsettings.js` file, use the following format to configure the SMTP server:

```json
{
  Email": {
    "SmtpServer": "smtp.office365.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@outlook-or-hotmail.com",
    "SenderName": "Invoicika Team",
    "SenderPassword": "your-app-password"
  }
}
```

![Description](https://i.imgur.com/wrV0y1L.png)

## How to Install (with Docker)

Make sure you have Docker Desktop installed. Then run

```bash
docker-compose up
```

You might see the seeder failed in docker compose log. To make the seeder happend, from your Docker Desktop, stop the backend container and run it again from Invoicika.
Open your browser and navigate to `http://localhost:4444` for the frontend.
Login with `username: admin1, password: admin1` as admin or `username: employee1, password: employee1` as employee. The backend is at `http://localhost:5000/swagger/index.html`

![Description](https://i.imgur.com/vNY5TTM.png)

## Contributing

We welcome contributions! Please submit a pull request or report issues. Ensure your code follows the project guidelines and passes all tests before submission.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Let me know if you'd like any further modifications!
