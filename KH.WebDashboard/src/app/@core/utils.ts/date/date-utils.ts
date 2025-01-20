import { format, parseISO } from "date-fns";

export function generateRandomNumberBasedOnDate(): number {
  const currentDate = new Date();
  const timestamp = currentDate.getTime();
  const randomFraction = Math.random();
  return Math.floor(timestamp * randomFraction);
}

export function getCurrentDateAsNumber(): number {
  const currentDate = new Date();
  const year = currentDate.getFullYear();
  const month = String(currentDate.getMonth() + 1).padStart(2, '0');
  const day = String(currentDate.getDate()).padStart(2, '0');

  return Number(`${year}${month}${day}`);
}

export function formatDateFns(dateString: string, dateFormat: string): string {
  try {
      const parsedDate = parseISO(dateString); // Parse the date string
      return format(parsedDate, dateFormat);  // Format the parsed date
  } catch (error) {
      console.error('Invalid date string:', dateString);
      throw error; // Handle invalid date strings appropriately
  }
}
