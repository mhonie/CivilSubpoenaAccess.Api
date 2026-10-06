import {
  Typography
} from "@mui/material";

import {
  Bar,
  BarChart,
  CartesianGrid,
  Legend,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis
} from "recharts";

export type BarChartSeries = {
  dataKey: string;
  name: string;
  color: string;
};

export type PaymentTypeBarChartDataRow = {
  month: string;
  [key: string]: string | number;
};

type PaymentTypeBarChartProps = {
  title: string;
  data: PaymentTypeBarChartDataRow[];
  series: BarChartSeries[];
};

export default function PaymentTypeBarChart({
  title,
  data,
  series
}: PaymentTypeBarChartProps) {
  return (
    <>
      <Typography variant="h5">
        {title}
      </Typography>

      <ResponsiveContainer
        width="100%"
        height={500}
      >
        <BarChart
          data={data}
          margin={{
            top: 20,
            right: 30,
            left: 20,
            bottom: 50
          }}
          barCategoryGap="20%"
          barGap={2}
        >
          <CartesianGrid strokeDasharray="3 3" />

          <XAxis dataKey="month" />

          <YAxis />

          <Tooltip
            wrapperStyle={{
              fontSize: "0.75rem",
              lineHeight: "1.2",
              paddingTop: 8
            }}
          />

          <Legend
            wrapperStyle={{
              fontSize: "0.75rem",
              lineHeight: "1.2",
              paddingTop: 8
            }}
          />

          {series.map(seriesItem => (
            <Bar
              key={seriesItem.dataKey}
              dataKey={seriesItem.dataKey}
              name={seriesItem.name}
              fill={seriesItem.color}
            />
          ))}
        </BarChart>
      </ResponsiveContainer>
    </>
  );
}