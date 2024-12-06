import React from "react";
import Container from "../components/reusable-layout-components/container";
import Row from "../components/reusable-layout-components/grid-row";
import Column from "../components/reusable-layout-components/grid-column";
import FilterContainer from "../components/reusable-layout-components/container";
import CardContainer from "../components/reusable-layout-components/container";

const PageContainer: React.FunctionComponent = () => {
    return (
        <Container className="custom-container px-5 py-3 navigation-margin">
            <Row>
                <Column size="col-4 col-lg-10">
                    <FilterContainer>
                        <p>Filter Test</p>
                    </FilterContainer>
                </Column>
                <Column size="col-8 col-lg-10">
                    <CardContainer>
                        <p>Card Test</p>
                    </CardContainer>
                </Column>
            </Row>
        </Container>
    );
};

export default PageContainer;
