![Results Table](https://github.com/eamelano/AssignRef/blob/main/react/assets/images/testResultsTable.jpg)

import React from "react";
import * as Feather from "react-feather";
import { Dropdown } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import { Row } from "react-bootstrap";
import DotBadge from "./DotBadge";
import { format } from "date-fns";

export default function TestsTable({ rowData }) {
  const customToggle = React.forwardRef(({ children, onClick }, ref) => (
    <Link
      to="#"
      ref={ref}
      onClick={(e) => {
        e.preventDefault();
        onClick(e);
      }}
    >
      {children}
    </Link>
  ));

  const indicatorTypes = {
    1: "success",
    2: "dark",
    3: "primary",
    4: "warning",
    5: "danger",
  };

  const navigate = useNavigate();

  const onClick = () => {
    let instanceId = rowData.id;
    false && instanceId;
    false && navigate(`/test/1/preview`);
  };

  return (
    <>
      <tbody role="rowgroup" className="text-center">
        <tr role="row">
          <th scope="row">
            <h4 className="mt-3 d-flex justify-content-center">
              {rowData.user.id}
            </h4>
          </th>

          <td role="cell">
            <Row>
              <h4 className="mb-0 mt-3">
                {rowData.user.firstName} {rowData.user.lastName}
              </h4>
            </Row>
          </td>
          <td role="cell">
            <div className="d-flex align-items-center justify-content-center mt-3">
              <h5>
                {format(new Date(rowData.dateCreated), "'Started' MMM dd yyyy")}
              </h5>
            </div>
          </td>
          <td role="cell">
            <div className="d-flex align-items-center justify-content-center mt-3">
              <h4>{rowData.test.name}</h4>
            </div>
          </td>
          <td role="cell">
            <h4 className="mt-3"> {rowData.testType.name} </h4>
          </td>
          <td role="cell">
            <h4 className="d-flex align-items-center justify-content-center mt-3">
              <DotBadge color={indicatorTypes[rowData.status.id]}></DotBadge>
              {rowData.status.name}
            </h4>
          </td>
          <td>
            <h4 className="mt-3">
              {" "}
              {Math.trunc(
                (rowData.correctAnswers / rowData.questionCount) * 100
              )}
              % ({rowData.correctAnswers}/{rowData.questionCount}){" "}
            </h4>
          </td>
          <td role="cell">
            <Dropdown>
              <Dropdown.Toggle as={customToggle}>
                <Feather.MoreVertical
                  size="25px"
                  className="text-secondary mt-2"
                />
              </Dropdown.Toggle>
              <Dropdown.Menu align="end">
                <Dropdown.Header>SETTINGS</Dropdown.Header>
                <Dropdown.Item onClick={onClick}>
                  <Feather.Eye className="text-info" /> View More
                </Dropdown.Item>
                <Dropdown.Item>
                  <Feather.Trash2 className="text-danger" /> Remove
                </Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          </td>
        </tr>
      </tbody>
    </>
  );
}

TestsTable.propTypes = {
  rowData: PropTypes.shape({
    id: PropTypes.number.isRequired,
    questionCount: PropTypes.number.isRequired,
    correctAnswers: PropTypes.number.isRequired,
    dateCreated: PropTypes.string.isRequired,
    user: PropTypes.shape({
      id: PropTypes.number.isRequired,
      firstName: PropTypes.string.isRequired,
      lastName: PropTypes.string.isRequired,
    }).isRequired,
    test: PropTypes.shape({
      name: PropTypes.string.isRequired,
    }).isRequired,
    testType: PropTypes.shape({
      name: PropTypes.string.isRequired,
    }).isRequired,
    status: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
  }),
  children: PropTypes.element,
  onClick: PropTypes.func,
};
