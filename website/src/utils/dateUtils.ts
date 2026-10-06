export function toUtcStartDate(date: string): string {
  return `${date}T00:00:00Z`;
}

export function toUtcEndDate(date: string): string {
  return `${date}T23:59:59Z`;
}

export function isValidReportDate(date: string): boolean {

// the date has to be "something"
if (!date) { return false; }

// the date has to be yyyy-dd-mm
const datePattern = /^\d{4}-\d{2}-\d{2}$/;

if (!datePattern.test(date)) {
  return false;
}

// the year can't be before 1900
const year = Number(date.substring(0, 4));

if (year < 1900) { return false; }

// the date can't be in the future
const parsedDate = new Date(`${date}T00:00:00Z`);

if (Number.isNaN(parsedDate.getTime())) {
  return false;
}

const today = new Date();

today.setHours(0, 0, 0, 0);

return parsedDate <= today;
}

export function areValidReportDates(
filingStartDate: string,
filingEndDate: string,
approvedStartDate: string,
approvedEndDate: string
): boolean {

return (
  isValidReportDate(filingStartDate) &&
  isValidReportDate(filingEndDate) &&
  isValidReportDate(approvedStartDate) &&
  isValidReportDate(approvedEndDate)
);
}

export function areValidChartDates(
filingStartDate: string,
filingEndDate: string
): boolean {

return (
  isValidReportDate(filingStartDate) &&
  isValidReportDate(filingEndDate)
);
}

export function optionalDateRangeError(
startDate: string,
endDate: string,
dateType: string
): string | null {

if (
  startDate && !isValidReportDate(startDate)

  || endDate && !isValidReportDate(endDate)
) {
  return `${dateType} dates must be valid dates between 01/01/1900 and today.`;
}

if (startDate && endDate && startDate > endDate) {
  return `${dateType} start date cannot be after ${dateType.toLowerCase()} end date.`;
}

return null;
}

export function oneWeekAgo(): string {
  const date = new Date();

  date.setUTCDate(date.getUTCDate() - 7);

  return date.toISOString().split("T")[0];
}

export function firstDayOfCurrentYear(): string {
const today = new Date();

return `${today.getFullYear()}-01-01`;
}

export function today(): string {
return new Date().toISOString().split("T")[0];
}

import dayjs from "dayjs";

export function formatDate(value?: string | null): string {

  if (!value) { return ""; }

  return dayjs(value).format("MM/DD/YYYY");
}

function isLeapYear(year: number): boolean
{
  return (
    (
      year % 4 == 0

      && year % 100 != 0
    )

    || year % 400 == 0
  );
}

export function sameDateInPreviousYear(date: string): string
{
  const dateParts = /^(\d{4})-(\d{2})-(\d{2})$/.exec(date);

  if (!dateParts)

    return "";

  const previousYear = Number(dateParts[1]) - 1;

  const month = dateParts[2];

  const day = month == "02"

              && dateParts[3] == "29"

              && !isLeapYear(previousYear)

    ? "28"

    : dateParts[3];

  return `${previousYear}-${month}-${day}`;
}
