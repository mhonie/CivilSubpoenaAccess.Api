import type {
  PaymentTypeCountsByPeriod
} from "../../api/subpoenaCounts";

import PaymentTypeBarChart, {
  type BarChartSeries
} from "../../components/charts/PaymentTypeBarChart";


const SERIES: BarChartSeries[] = [
  {
    dataKey: "walkIn",
    name: "Walk-Ins",
    color: "#6d4c41"
  },
  {
    dataKey: "cityLaw",
    name: "City Law",
    color: "#7b1fa2"
  },
  {
    dataKey: "citySolicitor",
    name: "City Solicitor",
    color: "#00838f"
  },
  {
    dataKey: "inFormaPauperis",
    name: "IFP",
    color: "#616161"
  }
];

type SubpoenasByOfflinePaymentTypeProps = {
  paymentTypeCountsByPeriod: PaymentTypeCountsByPeriod[];
};

export default function SubpoenasByOfflinePaymentType
(
  {
    paymentTypeCountsByPeriod
  }: SubpoenasByOfflinePaymentTypeProps
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

    walkIn: period.paymentTypeCount.walkIn,

    cityLaw: period.paymentTypeCount.cityLaw,

    citySolicitor: period.paymentTypeCount.citySolicitor,

    inFormaPauperis: period.paymentTypeCount.inFormaPauperis
  }));

  return (
    <PaymentTypeBarChart
      title="Subpoenas by Payment Type (Other)"
      data={chartData}
      series={SERIES}
    />
  );
}