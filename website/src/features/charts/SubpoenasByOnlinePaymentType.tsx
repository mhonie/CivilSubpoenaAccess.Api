import type {
  PaymentTypeCountsByPeriod
} from "../../api/subpoenaCounts";

import PaymentTypeBarChart, {
  type BarChartSeries
} from "../../components/charts/PaymentTypeBarChart";


const SERIES: BarChartSeries[] = [
  {
    dataKey: "americanExpress",
    name: "AMEX",
    color: "#1976d2"
  },
  {
    dataKey: "discover",
    name: "Discover",
    color: "#ef6c00"
  },
  {
    dataKey: "mastercard",
    name: "Mastercard",
    color: "#d32f2f"
  },
  {
    dataKey: "visa",
    name: "Visa",
    color: "#2e7d32"
  }
];

type SubpoenasByOnlinePaymentTypeProps = {
  paymentTypeCountsByPeriod: PaymentTypeCountsByPeriod[];
};

export default function SubpoenasByOnlinePaymentType
(
  {
    paymentTypeCountsByPeriod
  }: SubpoenasByOnlinePaymentTypeProps
)
{
  const chartData = paymentTypeCountsByPeriod.map(period => ({
    month: new Date(period.startDate).toLocaleDateString(
      "en-US",

      {
        month: "short",
        year: "2-digit",
        timeZone: "UTC"
      }
    ),

    americanExpress: period.paymentTypeCount.americanExpress,

    discover: period.paymentTypeCount.discoverCard,

    mastercard: period.paymentTypeCount.mastercard,

    visa: period.paymentTypeCount.visaCard
  }));

  return (
    <PaymentTypeBarChart
      title="Subpoenas by Payment Type (Credit Card)"
      data={chartData}
      series={SERIES}
    />
  );
}