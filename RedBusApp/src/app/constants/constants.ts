export const API_URL = "http://localhost:5059/api";

export const RED_BUS_CONTROLLER = {
  MASTER: 'Master',
  ADMIN: 'Admin',
  USER: 'User',
  AUTH: 'Auth',
};

export const MASTER_CONTROLLER = {
  TRENDING_ROUTES: 'TrendingRoutes',
  SEARCH_BUSES: 'SearchBuses',
  GET_ALL_LOCATIONS: 'GetAllLocations',
  GET_ALL_BUSES: 'GetAllBuses',
  GET_BUS_DETAILS_BY_ID: 'GetBusDetailsById'
};

export const ADMIN_CONTROLLER = {
  ADD_LOCATION: 'AddLocation',
  UPDATE_LOCATION: 'UpdateLocation',
  DELETE_LOCATION: 'DeleteLocation',
  ADD_BUS: 'AddBus',
  UPDATE_BUS: 'UpdateBus',
  DELETE_BUS: 'DeleteBus',
};

export const USER_CONTROLLER = {
  GET_ALL_BOOKINGS_BY_USER_ID: 'GetAllBookingsByUserId',
  CREATE_BOOKING: 'CreateBooking',
  CANCEL_BOOKING: 'CancelBooking',
};

export const AUTH_CONTROLLER = {
  REGISTER: 'Register',
  LOGIN: 'Login',
  REFRESH: 'Refresh',
  LOGOUT: 'Logout',
};
