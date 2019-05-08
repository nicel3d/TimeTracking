import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import moment from 'moment'

export const filterDefault: FilterRequest = new FilterRequest({
  begDate: moment().subtract(7, 'days').toDate(),
  endDate: moment().toDate(),
  begHour: 8,
  endHour: 17
})
