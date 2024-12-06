import React, {ReactNode} from "react";

interface CardContainerProps {
    children: ReactNode;
}

const CardContainer: React.FunctionComponent<CardContainerProps> = ({children}) => {
    return (
        <div className="d-flex flex-column">
            {children}
        </div>
    );
}

export default CardContainer;