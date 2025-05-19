# User Guide

This guide provides instructions for using the Hotel Management System.

## Getting Started

1. Launch the application by running `dotnet run --project HotelManagementSystem.App` or by executing the compiled binary
2. The application will open to the dashboard displaying current hotel statistics

## Room Management

### Adding a Room

1. Navigate to the Rooms section from the main menu
2. Click the "Add Room" button
3. Fill in the room details:
   - Room Number (required)
   - Room Type (Single, Double, Twin, Suite, Deluxe, Family)
   - Capacity
   - Price per night
   - Description (optional)
4. Click "Save" to add the room

### Editing a Room

1. Navigate to the Rooms section
2. Find the room you want to edit in the list
3. Click the "Edit" button next to the room
4. Modify the room details as needed
5. Click "Save" to update the room information

## Customer Management

### Adding a Customer

1. Navigate to the Customers section from the main menu
2. Click the "Add Customer" button
3. Fill in the customer details:
   - First Name (required)
   - Last Name (required)
   - Email Address
   - Phone Number
   - Address information
4. Click "Save" to add the customer

### Finding a Customer

1. Navigate to the Customers section
2. Use the search box to filter customers by name
3. Click on a customer to view their details

## Reservation Management

### Creating a Reservation

1. Navigate to the Reservations section from the main menu
2. Click the "New Reservation" button
3. Select or search for a customer
4. Choose available room(s)
5. Set check-in and check-out dates
6. Add any special requests
7. Click "Create Reservation" to confirm

### Check-in Process

1. Navigate to the Reservations section
2. Find the reservation by searching for the customer name or room number
3. Click on the reservation to view details
4. Click the "Check In" button to mark the guest as checked in

### Check-out Process

1. Navigate to the Reservations section
2. Find the customer's active reservation
3. Click the "Check Out" button
4. Review and finalize any changes to the bill
5. Confirm check-out 