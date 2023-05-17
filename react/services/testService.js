import axios from "axios";
import {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX,
} from "./serviceHelpers";

const testService = { testUrl: `${API_HOST_PREFIX}/api/tests` };

testService.getAllResults = (pageIndex, pageSize) => {
  const config = {
    method: "GET",
    url: `${testService.testUrl}/results/?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

testService.searchResults = (
  pageIndex,
  pageSize,
  query,
  startDate,
  endDate
) => {
  const config = {
    method: "GET",
    url: `${testService.testUrl}/results/search?pageIndex=${pageIndex}&pageSize=${pageSize}&query=${query}&startDate=${startDate}&endDate=${endDate}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

testService.CreateInstance = (payload) => {
  const config = {
    method: "POST",
    url: `${testService.testUrl}/results`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

testService.getById = (id) => {
  const config = {
    method: "GET",
    url: `${testService.testUrl}/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export default testService;
