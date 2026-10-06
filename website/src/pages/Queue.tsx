import BottomButtons from "../components/BottomButtons";
import CopyrightFooter from "../components/CopyrightFooter";
import TopBanner from "../components/TopBanner";
import ReviewQueue from "../features/review-queue/SubpoenasForReview";

import { Typography } from "@mui/material";

export default function Queue() {
  return (
    <>
      <TopBanner/>
      <Typography variant="h4" sx={{mb: 2}}>
        Search & Review Queue
      </Typography>
      <ReviewQueue />
      <BottomButtons />
      <CopyrightFooter/>
    </>
  );
}
