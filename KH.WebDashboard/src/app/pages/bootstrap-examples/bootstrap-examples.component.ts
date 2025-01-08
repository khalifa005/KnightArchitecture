import { Component } from '@angular/core';

@Component({
  selector: 'app-bootstrap-examples',
  templateUrl: './bootstrap-examples.component.html',
  styleUrl: './bootstrap-examples.component.scss'
})
export class BootstrapExamplesComponent {
  items = Array.from({ length: 50 }, (_, i) => `Content Line ${i + 1}`);

  
  products = [
    { name: 'Laptop', price: '$800', quantity: 10 },
    { name: 'Phone', price: '$500', quantity: 20 },
    { name: 'Tablet', price: '$300', quantity: 15 },
    { name: 'Monitor', price: '$200', quantity: 8 },
    { name: 'Keyboard', price: '$50', quantity: 25 },
    { name: 'Mouse', price: '$20', quantity: 30 },
    { name: 'Headphones', price: '$100', quantity: 12 },
    { name: 'Speakers', price: '$150', quantity: 18 },
    { name: 'Printer', price: '$120', quantity: 7 },
    { name: 'Scanner', price: '$130', quantity: 6 },
    { name: 'Router', price: '$90', quantity: 15 },
    { name: 'Switch', price: '$70', quantity: 10 },
    { name: 'Hard Drive', price: '$200', quantity: 14 },
    { name: 'SSD', price: '$150', quantity: 10 },
    { name: 'RAM', price: '$80', quantity: 12 },
    { name: 'Graphics Card', price: '$500', quantity: 5 },
    { name: 'Motherboard', price: '$250', quantity: 8 },
    { name: 'Processor', price: '$300', quantity: 9 },
    { name: 'Power Supply', price: '$100', quantity: 11 },
    { name: 'Case', price: '$60', quantity: 20 }
  ];

}
