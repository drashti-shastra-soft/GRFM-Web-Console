# GRFM Web Console

GRFM Web Console is a responsive ASP.NET web console for furnishing shop owners. It lets shop owners create detailed quotations for curtains, upholstery, and custom furnishing work, and share them with customers via WhatsApp, email, or PDF. Shop owners can also sell readymade items such as pillows, bedsheets, and mattresses, manage stock, orders, and customers, and view sales reports via the GRFM Web API. The solution is organized into separate Domain, Infrastructure, Service, and Web Console projects.

## Features

- **Authentication** — sign-in page and authentication flow for shop owners and staff.
- **Dashboard** — landing page with key business highlights after entering the console.
- **Quotation management** — create, list, view, edit, and delete detailed quotations for curtains, upholstery, and custom furnishing work.
- **Quotation sharing** — share quotations with customers via WhatsApp, email, or downloadable PDF.
- **Custom furnishing work** — line items with measurements, fabric selection, stitching/installation charges, and custom notes.
- **Readymade products** — manage pillows, bedsheets, mattresses, and other readymade items with categories, pricing, and images.
- **Stock management** — track stock levels, stock-in/stock-out, and low-stock alerts for readymade items.
- **Order management** — create, list, view, and update orders for readymade items.
- **Customer management** — customer records, contact details, and order/quotation history.
- **Sales reports** — sales summaries, product-wise and period-wise reports via the GRFM Web API.
- **Shared web UI** — Razor views, shared layout, pagination helper, and client-side validation.
- **Responsive design** — mobile-ready layout for shop owners using phones and tablets.

> Feature descriptions are based on the controllers, domain models, services, and views present in this repository. Actual permissions and backend behavior may depend on the connected GRFM Web API.

## Technology stack

- C# and .NET Framework 4.8 (or `<your target framework>`)
- ASP.NET MVC with Razor views
- Visual Studio solution (`GRFMWebConsole.sln`)
- Dependency injection configured through Unity
- HTTP client/service layer for communicating with the GRFM Web API
- PDF generation via `<iTextSharp / Rotativa / other library>`
- WhatsApp sharing via `<WhatsApp Click-to-Chat / WhatsApp Business API>`
- Email sending via `<SMTP / SendGrid / other provider>`
- HTML, CSS, Bootstrap, JavaScript, and jQuery-based MVC views

## Solution structure

```text
V4/
├── GRFMWebConsole.sln
├── GRFMWebConsole/                       # ASP.NET MVC web application
│   ├── Controllers/                      # MVC request handling
│   ├── Views/                            # Razor UI pages
│   ├── App_Start/                        # Routes, filters, Unity setup
│   ├── Config/                           # Environment-specific app settings
│   └── Web.config
├── GRFMWebConsole.Domain/                # Domain models and list/pagination models
├── GRFMWebConsole.Infrastructure/        # Service interfaces, helpers, constants, exceptions
└── GRFMWebConsole.Service/               # Service implementations and API communication